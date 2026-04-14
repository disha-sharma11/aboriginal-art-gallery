pipeline {
    agent any

    environment {
        DOTNET_CLI_TELEMETRY_OPTOUT = '1'
        ASPNETCORE_ENVIRONMENT = 'Development'
        REACT_APP_API_BASE_URL = 'http://localhost:5141/api'
    }

    stages {
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

        stage('Deploy Staging') {
            steps {
                sh 'docker compose -p aboriginal-art-gallery-staging -f docker-compose.staging.yml down || true'
                sh 'docker compose -p aboriginal-art-gallery-staging -f docker-compose.staging.yml up --build -d'

            }
        }

        stage('Smoke Test Staging') {
            steps {
                sh '''
                    for i in {1..12}; do
                      if curl --fail --silent http://localhost:5142/health; then
                        exit 0
                      fi
                      echo "Waiting for staging health endpoint..."
                      sleep 5
                    done
                    echo "Staging health check failed."
                    exit 1
                '''
            }
        }

        stage('Approve Production Release') {
            steps {
                input message: 'Deploy this build to production?', ok: 'Release'
            }
        }

        stage('Release Production') {
            steps {
                sh 'docker compose -p aboriginal-art-gallery-production -f docker-compose.prod.yml down || true'
                sh 'docker compose -p aboriginal-art-gallery-production -f docker-compose.prod.yml up --build -d'
            }
        }

        stage('Smoke Test Production') {
            steps {
                sh '''
                    for i in {1..12}; do
                      if curl --fail --silent http://localhost:5143/health; then
                        exit 0
                      fi
                      echo "Waiting for production health endpoint..."
                      sleep 5
                    done
                    echo "Production health check failed."
                    exit 1
                '''
            }
        }

        stage('Monitoring Check') {
            steps {
                sh '''
                    {
                      echo "Staging health:"
                      curl --fail --silent http://localhost:5142/health
                      echo
                      echo "Production health:"
                      curl --fail --silent http://localhost:5143/health
                      echo
                    } > monitoring-check.txt
                '''
            }
        }
    }

    post {
        always {
            echo 'Pipeline finished.'
            archiveArtifacts artifacts: 'security-dotnet.txt,security-npm.json,monitoring-check.txt', fingerprint: true
        }
        success {
            echo 'All 7 pipeline stages passed, including staging deployment, production release, and monitoring checks.'
            mail to: 'itsdisha6@gmail.com',
                 subject: "SUCCESS: ${env.JOB_NAME} #${env.BUILD_NUMBER}",
                 body: "Pipeline succeeded.\nJob: ${env.JOB_NAME}\nBuild: ${env.BUILD_NUMBER}\nURL: ${env.BUILD_URL}"
        }
        failure {
            echo 'Pipeline failed. Check the stage logs.'
            mail to: 'itsdisha6@gmail.com',
                 subject: "FAILURE: ${env.JOB_NAME} #${env.BUILD_NUMBER}",
                 body: "Pipeline failed.\nJob: ${env.JOB_NAME}\nBuild: ${env.BUILD_NUMBER}\nURL: ${env.BUILD_URL}"
        }
    }
}
