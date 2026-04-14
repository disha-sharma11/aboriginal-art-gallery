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
                script 
                {
                    def scannerHome = tool 'SonarScanner'
                    withSonarQubeEnv('SonarCloud') 
                    {
                        sh "${scannerHome}/bin/sonar-scanner"
                    }
                }
            }
        }
        
        // stage('Quality Gate') {
        //     steps {
        //         timeout(time: 10, unit: 'MINUTES') {
        //             waitForQualityGate abortPipeline: true
        //         }
        //     }
        // }

        stage('Security') {
            steps {
                dir('backend') {
                    sh 'dotnet list AboriginalArtGallery.slnx package --vulnerable --include-transitive > ../security-dotnet.txt || true'
                }
                dir('frontend') {
                    sh 'npm audit --audit-level=high --json > ../security-npm.json || true'
                }
            }
        }


    }

    post {
        always {
            echo 'Pipeline finished.'
            archiveArtifacts artifacts: 'security-dotnet.txt,security-npm.json', fingerprint: true

        }
        success {
            echo 'Build, test, and code quality stages passed.'
        }
        failure {
            echo 'Pipeline failed. Check the stage logs.'
        }
    }
}
