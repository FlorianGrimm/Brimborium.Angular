# initial steps

1. folder structur

    ```cmd
    set root=D:\github.com\FlorianGrimm\Brimborium.Angular\Brimborium.DefineAPI\
    mkdir %root%
    cd /D %root%
    dotnet new sln
    mkdir %root%\src
    mkdir %root%\documentation
    mkdir %root%\progress
    mkdir %root%\samplesimple
    mkdir %root%\samplei18n
    ```

2. create ClientApp

    ```cmd
    cd /D %root%\src
    ng new ClientApp
    ```

3. init @angular/localize

    ```cmd
    cd %root%\src\ClientApp
    ng add @angular/localize
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

6. todo

    ```cmd
    cd %root%\src\ClientApp
    ng add @angular/localize
    ```

eof
cd /D 