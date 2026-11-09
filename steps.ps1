<#
pwsh -File steps.ps1
#>
[string]$name = "Brimborium.Angular"
[string]$root = $PWD.Path

$doc=[System.IO.Path]::Combine($root, "doc")
[System.IO.Directory]::CreateDirectory($doc)

$src=[System.IO.Path]::Combine($root, "src")
[System.IO.Directory]::CreateDirectory($src)

$test=[System.IO.Path]::Combine($root, "test")
[System.IO.Directory]::CreateDirectory($test)

[string]$sln = [System.IO.Path]::Combine($root, "$($name).slnx")
if (-not (Test-Path -LiteralPath $sln)){
    . dotnet new sln
}

[string]$globaljson = [System.IO.Path]::Combine($root, "global.json")
if (-not (Test-Path -LiteralPath $globaljson)){
    . dotnet new globaljson
}

[string]$gitignore = [System.IO.Path]::Combine($root, ".gitignore")
if (-not (Test-Path -LiteralPath $gitignore)){
    . dotnet new gitignore
}

[string]$packagesprops = [System.IO.Path]::Combine($root, "Directory.Packages.props")
if (-not (Test-Path -LiteralPath $packagesprops)){
    . dotnet new packagesprops
    [string]$Content=Get-Content $packagesprops -Raw 
    $Content=$Content.Replace("<ManagePackageVersionsCentrally>true</ManagePackageVersionsCentrally>", "<ManagePackageVersionsCentrally>false</ManagePackageVersionsCentrally>")
    Set-Content -Path $packagesprops -Value $Content
}

[string]$buildprops = [System.IO.Path]::Combine($root, "Directory.Build.props")
if (-not (Test-Path -LiteralPath $buildprops)){
    . dotnet new buildprops   --use-artifacts
}

$nameContract="$($name).Contract"
$outputContract=[System.IO.Path]::Combine($root, "src", "$($name).Contract")
if (-not (Test-Path -LiteralPath $outputContract)){
    . dotnet new classlib --name $nameContract --output $outputContract
    . dotnet solution $sln add $outputContract --solution-folder "src"
}



$outputMain=[System.IO.Path]::Combine($root, "src", $name)
if (-not (Test-Path -LiteralPath $outputMain)){
    . dotnet new webapi --name $name --output $outputMain --use-minimal-apis --auth Windows
    . dotnet solution $sln add $outputMain --solution-folder "src"

    . dotnet reference add $outputContract --project $outputMain
    . dotnet package add Injectio --project $outputContract
}

$nameWebUI="$($name)UI"
$outputWebUI=[System.IO.Path]::Combine($root, "src", $nameWebUI)
if (-not (Test-Path -LiteralPath $outputWebUI)){
    Set-Location $src
    [System.Environment]::CurrentDirectory=$src

    . pnpm install -g @angular/cli

    . ng new $nameWebUI --ai-config "vscode" --directory $nameWebUI --package-manager        "pnpm" --prefix                 "app" --routing                 --skip-git --skip-install --ssr false --standalone --strict --style "scss" --test-runner "vitest" --zoneless

    . pnpm install --dir $outputWebUI

    . dotnet new classlib --name $nameWebUI --output $outputWebUI
    . dotnet solution $sln add $outputWebUI --solution-folder "src"

    $outputWebUIwwwroot=[System.IO.Path]::Combine($outputWebUI, "wwwroot")
    [System.IO.Directory]::CreateDirectory($outputWebUIwwwroot)


    <#
        <ItemGroup>
            <FrameworkReference Include="Microsoft.AspNetCore.App" />
        </ItemGroup>    
    #>
}

Set-Location $outputWebUI
[System.Environment]::CurrentDirectory=$outputWebUI
. pnpm install --dir $outputWebUI

[string]$nameTests = "$($name).Tests"
$outputWebTests=[System.IO.Path]::Combine($root, "test", $nameTests)
if (-not (Test-Path -LiteralPath $outputWebTests)){
    . dotnet new classlib --name $nameTests --output $outputWebTests
    . dotnet solution $sln add $outputWebTests --solution-folder "test"
    . dotnet reference add $outputContract --project $outputWebTests
    . dotnet reference add $outputWeb --project $outputWebTests
    . dotnet package add TUnit --project $outputWebTests
}

[string]$nameUITests = "$($name).UITests"
$outputWebUITests=[System.IO.Path]::Combine($root, "test", $nameUITests)
if (-not (Test-Path -LiteralPath $outputWebUITests)){
    . dotnet new classlib --name $nameUITests --output $outputWebUITests
    . dotnet solution $sln add $outputWebUITests --solution-folder "test"
    . dotnet reference add $outputContract --project $outputWebUITests
    . dotnet reference add $outputWeb --project $outputWebUITests
    . dotnet package add TUnit --project $outputWebUITests
}

$outputDoc=[System.IO.Path]::Combine($root, "doc")
if (-not (Test-Path -LiteralPath $outputDoc)){
    . dotnet new webapi --name "doc" --output $outputDoc --use-minimal-apis --auth None
    . dotnet solution $sln add $outputDoc --in-root
}



<# #>
