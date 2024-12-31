# initial steps

1. folder structur

    ```cmd
    set root=D:\github.com\FlorianGrimm\Brimborium.Angular\Brimborium.Angular\
    mkdir %root%
    cd /D %root%
    dotnet new sln
    mkdir %root%\src
    mkdir %root%\documentation
    mkdir %root%\progress
    ```

2. create ClientApp

    ```cmd
    cd /D %root%\src
    ng new ClientApp
    ```

3. create WebApp

    ```cmd
    mkdir %root%\src\WebApp
    cd /D %root%\src\WebApp
    dotnet new webapp --auth Windows --use-program-main
    cd /D %root%
    dotnet solution add %root%\src\WebApp --solution-folder src
    ```

4. build ClientApp

    ```cmd
    cd %root%\src\ClientApp
    npm run watch
    ```

5. build WebApp

    ```cmd
    cd %root%\src\WebApp
    dotnet build
    ```

6. extract-i18n

    ```cmd
    ng extract-i18n --output-path src/locale --out-file messages.en-US.xlf
    ```

eof