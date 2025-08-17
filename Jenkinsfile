pipeline {
    agent any

    environment {
        // Defines the name of the .NET solution file.
        SOLUTION_FILE = 'TestProject1.sln'
        // Defines the name of the test project.
        TEST_PROJECT_FILE = 'TestProject1/TestProject1.csproj'
        // Defines the configuration for building the project.
        BUILD_CONFIGURATION = 'Release'
    }

    stages {
        stage('Checkout') {
            steps {
                // Fetches the source code from your Git repository.
                git 'https://github.com/rameshbaw/TestProject1.git'
            }
        }

        stage('Build') {
            steps {
                // Restores NuGet packages, cleans the previous build, and builds the solution.
                bat "dotnet restore ${env.SOLUTION_FILE}"
                bat "dotnet clean ${env.SOLUTION_FILE} --configuration ${env.BUILD_CONFIGURATION}"
                bat "dotnet build ${env.SOLUTION_FILE} --configuration ${env.BUILD_CONFIGURATION} --no-restore"
            }
        }

        stage('Test') {
            steps {
                // Executes the Selenium tests using the specified test project.
                // The --logger parameter generates a test results file.
                bat "dotnet test ${env.TEST_PROJECT_FILE} --configuration ${env.BUILD_CONFIGURATION} --no-build --logger \\"trx;LogFileName=test_results.trx""
            }
        }

        stage('Publish Test Results') {
            steps {
                // Publishes the test results to Jenkins for visualization and analysis.
                // The MSTest plugin is used to parse the .trx file.
                mstest testResultsFile: '**/test_results.trx', keepLongStdio: true
            }
        }

        // --- Choose one of the deployment stages below ---

        stage('Deploy to IIS') {
            steps {
                script {
                    // This is an example of deploying to an IIS server.
                    // You will need to customize the PowerShell script to your specific environment.
                    withCredentials([usernamePassword(credentialsId: 'iis-credentials', usernameVariable: 'USERNAME', passwordVariable: 'PASSWORD')]) {
                        powershell(returnStdout: true, script: '''
                            $publishPath = ".\\YourWebApp\\bin\\${env.BUILD_CONFIGURATION}\\netcoreapp3.1\\publish"
                            $destination = "\\\\your-iis-server\\your-website-folder"

                            # Creates the publish directory
                            dotnet publish YourWebApp/YourWebApp.csproj --configuration ${env.BUILD_CONFIGURATION} --output $publishPath

                            # Example of copying files to the IIS server
                            Copy-Item -Path "$publishPath\\*" -Destination $destination -Recurse -Force

                            # You might need to add steps to stop/start the app pool, etc.
                            Write-Host "Deployment to IIS completed."
                        ''')
                    }
                }
            }
        }

        stage('Deploy to Azure App Service') {
            steps {
                script {
                    // This is an example of deploying to an Azure App Service.
                    // You will need the Azure CLI plugin installed in Jenkins.
                    withCredentials([azureServicePrincipal('your-azure-sp-credentials-id')]) {
                        // Publish the application
                        bat "dotnet publish YourWebApp/YourWebApp.csproj --configuration ${env.BUILD_CONFIGURATION} --output ./publish"

                        // Login to Azure and deploy
                        powershell(returnStdout: true, script: '''
                            az login --service-principal -u $env:AZURE_CLIENT_ID -p $env:AZURE_CLIENT_SECRET --tenant $env:AZURE_TENANT_ID
                            az webapp deploy --resource-group YourResourceGroup --name YourWebAppName --src-path ./publish --type zip
                        ''')
                    }
                }
            }
        }
    }

    post {
        // This block executes after all stages have completed.
        always {
            // Clean up the workspace.
            cleanWs()
        }
        success {
            // Send a notification for a successful build.
            echo 'Pipeline successfully completed.'
            // Example for sending an email notification.
            // mail to: 'your-email@example.com',
            //      subject: "SUCCESS: Pipeline '${currentBuild.fullDisplayName}'",
            //      body: "The pipeline has completed successfully."
        }
        failure {
            // Send a notification for a failed build.
            echo 'Pipeline failed.'
            // Example for sending an email notification.
            // mail to: 'your-email@example.com',
            //      subject: "FAILURE: Pipeline '${currentBuild.fullDisplayName}'",
            //      body: "The pipeline has failed. Check the logs: ${env.BUILD_URL}"
        }
    }
}
