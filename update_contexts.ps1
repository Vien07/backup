# Đường dẫn tới file chứa các tên context
$contextFile = "modules.json"

# Đường dẫn tới project
$projectPath = "./MasterAdmin/MasterAdmin.csproj"

# Kiểm tra xem file context có tồn tại không
if (-Not (Test-Path $contextFile)) {
    Write-Host "File $contextFile not exist!"
    exit 1
}

# Đọc và parse file JSON
$contexts = (Get-Content $contextFile | ConvertFrom-Json).contexts

# Lặp qua từng context và thực hiện lệnh update
foreach ($contextName in $contexts) {
    Write-Host "Updating database for context: $contextName"
    $result = dotnet ef database update --context $contextName --project $projectPath
    
    # Kiểm tra trạng thái lệnh vừa thực hiện
    if ($LASTEXITCODE -ne 0) {
        Write-Host "Update for context $contextName failure!"
	pause
        exit 1
    }
}

Write-Host "Succeed!"

pause