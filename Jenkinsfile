pipeline {
  agent {
    // Run inside a .NET + Chrome image (use the Dockerfile I gave earlier or a Windows agent with .NET + Chrome installed)
    docker {
      image 'ghcr.io/your-org/dotnet8-chrome:1.0'
      args '-u root -v $HOME/.nuget/packages:/root/.nuget/packages'
      reuseNode true
    }
  }

  options {
    timestamps()
    ansiColor('xterm')
    disableConcurrentBuilds()
    buildDiscarder(logRotator(numToKeepStr: '20'))
    timeout(time: 20, unit: 'MINUTES')
  }

  environment {
    DOTNET_CLI_TELEMETRY_OPTOUT = '1'
    DOTNET_NOLOGO               = '1'
    PATH                        = "$PATH:/root/.dotnet/tools"
  }

  stages {
    stage('Checkout') {
      steps {
        checkout scm
        sh 'git --no-pager log -1 --pretty=oneline'
      }
    }

    stage('Restore') {
      steps {
        sh 'dotnet restore TestProject1/TestProject1.sln'
      }
    }

    stage('Build') {
      steps {
        sh 'dotnet build TestProject1/TestProject1.sln --configuration Release --no-restore'
      }
    }

    stage('Test') {
      steps {
        sh """
          mkdir -p TestResults
          dotnet tool install --global trx2junit || true
          dotnet test TestProject1/TestProject1.csproj \
            --configuration Release --no-build \
            --logger "trx;LogFileName=test_results.trx"
          cp TestProject1/TestResults/test_results.trx TestResults/ || true
          trx2junit TestResults/*.trx || true
        """
      }
      post {
        always {
          junit allowEmptyResults: true, testResults: 'TestResults/*.xml'
          archiveArtifacts artifacts: 'TestResults/*.*', allowEmptyArchive: true
        }
      }
    }
  }

  post {
    success { echo '✅ Build & tests passed.' }
    failure { echo '❌ Build or tests failed.' }
    cleanup { cleanWs() }
  }
}
