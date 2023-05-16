pipeline {
    agent any

    stages {

        stage('Restore Dependencies') {
            steps {
                bat 'dotnet restore' // Restore the project dependencies
            }
        }

        stage('Build') {
            steps {
                bat 'dotnet build --configuration Release' // Build the project in Release mode
            }
        }

        stage('Test') {
            steps {
                bat 'dotnet test' // Run the tests
            }
        }

        stage('Artifacts') {
            steps {
                bat 'dotnet publish --configuration Release --output ./artifacts' // Publish the application
            }
        }
    }
}
