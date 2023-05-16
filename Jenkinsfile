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
                bat 'dotnet publish WebAPI --configuration Release --output ./Artifact-WebAPI' // Publish the application
				bat 'dotnet publish WebApp --configuration Release --output ./Artifact-WebApp' // Publish the application
            }
        }
    }
}
