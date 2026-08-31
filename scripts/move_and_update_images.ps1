<#
Script: move_and_update_images.ps1
Descrição:
- Move/renomeia imagens encontradas no projeto UI/wwwroot para wwwroot/images/products
- Gera um arquivo SQL (updates_images.sql) com UPDATEs para o campo CoverImageUrl na tabela Products
- Opcionalmente executa os UPDATEs se uma connection string for fornecida e -ExecuteSql for passado

Uso:
.
./scripts/move_and_update_images.ps1 [-ProjectRoot <path>] [-ExecuteSql] [-ConnectionString <conn>] [-DryRun]

Parâmetros:
-ProjectRoot: pasta raiz do repositório (padrão: pasta acima do script)
-DryRun: apenas mostra operações sem mover/executar SQL
-ExecuteSql: executa os UPDATEs no banco (requer ConnectionString)
-ConnectionString: connection string para o banco SQL Server
#>

param(
	[string]$ProjectRoot = (Split-Path -Parent $PSScriptRoot),
	[switch]$ExecuteSql,
	[string]$ConnectionString,
	[switch]$DryRun
)

Write-Host "Project root: $ProjectRoot"

$wwwroot = Join-Path $ProjectRoot "AtelieDosPontinhos.UI\wwwroot"
$destDir = Join-Path $wwwroot "images\products"

if (-not (Test-Path $wwwroot)) {
	Write-Error "wwwroot não encontrado em: $wwwroot"
	exit 1
}

if (-not (Test-Path $destDir)) {
	if (-not $DryRun) { New-Item -ItemType Directory -Path $destDir | Out-Null }
	Write-Host "Criado diretório: $destDir"
}

# Extensões a considerar
$extensions = @('*.png','*.jpg','*.jpeg','*.webp','*.gif')

# Função para normalizar nomes (remover espaços, acentos, deixar minúsculo)
function Remove-Accents {
	param([string]$s)
	$normalized = [string]::Concat($s.Normalize([Text.NormalizationForm]::FormD) | Where-Object { [Globalization.CharUnicodeInfo]::GetUnicodeCategory($_) -ne 'NonSpacingMark' })
	return $normalized
}

function SanitizeName {
	param([string]$name)
	$name = [System.IO.Path]::GetFileNameWithoutExtension($name)
	$ext = [System.IO.Path]::GetExtension($name)
	# Se ext vazia, tenta extrair do original
	$ext = if ($ext -eq '') { [System.IO.Path]::GetExtension($args[0]) } else { $ext }
	$name = Remove-Accents $name
	$name = $name -replace "[^a-zA-Z0-9\.-]","-"
	$name = $name -replace "-+","-"
	$name = $name.Trim('-').ToLowerInvariant()
	return "$name$ext"
}

$mapping = @()

foreach ($pattern in $extensions) {
	$files = Get-ChildItem -Path $wwwroot -Recurse -Include $pattern | Where-Object { $_.FullName -notmatch "\\wwwroot\\lib\\" }
	foreach ($f in $files) {
		$origRel = $f.FullName.Substring($wwwroot.Length).Replace('\\','/')
		$origName = $f.Name
		# manter a extensão do arquivo original
		$base = [System.IO.Path]::GetFileNameWithoutExtension($origName)
		$ext = [System.IO.Path]::GetExtension($origName)
		$sanitizedBase = Remove-Accents $base
		$sanitizedBase = $sanitizedBase -replace "[^a-zA-Z0-9\.-]","-"
		$sanitizedBase = $sanitizedBase -replace "-+","-"
		$sanitizedBase = $sanitizedBase.Trim('-').ToLowerInvariant()
		$newName = "${sanitizedBase}${ext.ToLowerInvariant()}"
		$newFull = Join-Path $destDir $newName
		$newRel = "/images/products/$newName"

		# evitar sobrescrever: se arquivo já existe, acrescenta sufixo incremental
		$i = 1
		$candidate = $newFull
		while (Test-Path $candidate) {
			$candidate = Join-Path $destDir ("${sanitizedBase}-${i}${ext.ToLowerInvariant()}")
			$i++
		}
		if ($candidate -ne $newFull) {
			$newName = [System.IO.Path]::GetFileName($candidate)
			$newFull = $candidate
			$newRel = "/images/products/$newName"
		}

		$mapping += [PSCustomObject]@{
			OldFull = $f.FullName
			OldRel = $origRel
			NewFull = $newFull
			NewRel = $newRel
			OldFileName = $origName
			NewFileName = $newName
		}
	}
}

if ($mapping.Count -eq 0) {
	Write-Host "Nenhuma imagem encontrada para mover. Verifique os padrões e caminhos."
	exit 0
}

# Mostrar resumo
Write-Host "Encontradas $($mapping.Count) imagens para processar:`n"
$mapping | ForEach-Object { Write-Host "`nDe: $($_.OldRel)`nPara: $($_.NewRel)" }

if ($DryRun) { Write-Host "DRY RUN - nenhum arquivo será movido nem SQL executado."; exit 0 }

# Mover arquivos
foreach ($m in $mapping) {
	try {
		Move-Item -Path $m.OldFull -Destination $m.NewFull -Force
		Write-Host "Movido: $($m.OldRel) -> $($m.NewRel)"
	} catch {
		Write-Warning "Falha ao mover $($m.OldRel): $_"
	}
}

# Gerar arquivo SQL
$sqlFile = Join-Path $ProjectRoot "updates_images.sql"
"-- Script gerado por move_and_update_images.ps1 - atualiza CoverImageUrl na tabela Products" | Out-File -FilePath $sqlFile -Encoding utf8
"BEGIN TRANSACTION;" | Out-File -FilePath $sqlFile -Encoding utf8 -Append

foreach ($m in $mapping) {
	# Usar LIKE para corresponder ao nome antigo (sem caminho) — ajustar conforme seu padrão no DB
	$oldNameEscaped = $m.OldFileName -replace "'","''"
	$newRelEscaped = $m.NewRel -replace "'","''"
	$sql = "UPDATE Products SET CoverImageUrl = '$newRelEscaped' WHERE CoverImageUrl LIKE '%$oldNameEscaped%';"
	$sql | Out-File -FilePath $sqlFile -Encoding utf8 -Append
}
"COMMIT;" | Out-File -FilePath $sqlFile -Encoding utf8 -Append
Write-Host "Arquivo SQL gerado: $sqlFile"

# Executar SQL se solicitado
if ($ExecuteSql) {
	if ([string]::IsNullOrEmpty($ConnectionString)) {
		Write-Error "ConnectionString é necessária para executar SQL. Use -ConnectionString '<cadena>'"
		exit 1
	}

	Add-Type -AssemblyName System.Data
	$conn = New-Object System.Data.SqlClient.SqlConnection $ConnectionString
	try {
		$conn.Open()
		Write-Host "Conectado ao banco. Executando atualizações..."
		$cmd = $conn.CreateCommand()
		$cmd.CommandText = Get-Content -Raw -Path $sqlFile
		$cmd.CommandTimeout = 600
		$cmd.ExecuteNonQuery() | Out-Null
		Write-Host "Atualizações aplicadas com sucesso."
	} catch {
		Write-Error "Erro ao executar SQL: $_"
	} finally {
		$conn.Close()
	}
}

Write-Host "Concluído." | Out-Null
