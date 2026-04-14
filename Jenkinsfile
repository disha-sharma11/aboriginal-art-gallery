pipeline {
    agent any

    environment {
        DOTNET_CLI_TELEMETRY_OPTOUT = '1'
        ASPNETCORE_ENVIRONMENT = 'Development'
        REACT_APP_API_BASE_URL = 'http://localhost:5141/api'
    }

    stages {
        stage('Checkout') {
            steps {
                checkout scm
            }
        }

        stage('Build Backend') {
            steps {
                dir('backend') {
                    sh 'dotnet restore AboriginalArtGallery.slnx'
                    sh 'dotnet build AboriginalArtGallery.slnx --no-restore'
                }
            }
        }

        stage('Build Frontend') {
            steps {
                dir('frontend') {
                    sh 'npm ci'
                    sh 'npm run build'
                }
            }
        }

        stage('Test Backend') {
            steps {
                dir('backend') {
                    sh 'dotnet test AboriginalArtGallery.slnx --no-build'
                }
            }
        }

        stage('Test Frontend') {
            steps {
                dir('frontend') {
                    sh 'CI=true npm test -- --watchAll=false'
                }
            }
        }
        
        stage('Code Quality') {
            steps {
                withSonarQubeEnv('SonarQube') {
                    sh '''
                        sonar-scanner
                    '''
                }
            }
        }

    }

    post {
        always {
            echo 'Pipeline finished.'
        }
        success {
            echo 'Build and test stages passed.'
        }
        failure {
            echo 'Pipeline failed. Check the stage logs.'
        }
    }
}
