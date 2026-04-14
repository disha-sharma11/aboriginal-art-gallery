pipeline {
    agent any

    environment {
        DOTNET_CLI_TELEMETRY_OPTOUT = '1'
        ASPNETCORE_ENVIRONMENT = 'Development'
        REACT_APP_API_BASE_URL = 'http://localhost:5141/api'
        IMAGE_TAG = "build-${BUILD_NUMBER}"
    }

    stages {
        stage('Build Backend') {
            steps {
                dir('backend') {
                    sh 'dotnet restore AboriginalArtGallery.slnx'
                    sh 'dotnet build AboriginalArtGallery.slnx --no-restore -c Release'
                    sh 'mkdir -p ../artifacts/backend'
                    sh 'dotnet publish AboriginalArtGallery.Api/AboriginalArtGallery.Api.csproj -c Release --no-build -o ../artifacts/backend'
                }
            }
        }

        stage('Build Frontend') {
            steps {
                dir('frontend') {
                    sh 'npm ci'
                    sh 'npm run build'
                    sh 'mkdir -p ../artifacts/frontend && cp -R build/. ../artifacts/frontend/'
                }
            }
        }

        stage('Test Backend') {
            steps {
                dir('backend') {
                    sh 'mkdir -p ../artifacts/test-results/backend'
                    sh 'dotnet test AboriginalArtGallery.slnx --no-build -c Release --logger "trx;LogFileName=backend-tests.trx" --results-directory ../artifacts/test-results/backend'
                }
            }
        }

        stage('Test Frontend') {
            steps {
                dir('frontend') {
                    sh 'CI=true npm test -- --watchAll=false --coverage'
                }
            }
        }
        
        stage('Code Quality') {
            steps {
                withSonarQubeEnv('SonarCloud') {
                    sh '''
                        dotnet tool update --global dotnet-sonarscanner || dotnet tool install --global dotnet-sonarscanner
                        export PATH="$PATH:$HOME/.dotnet/tools"

                        dotnet sonarscanner begin \
                            /k:"disha-sharma11_aboriginal-art-gallery" \
                            /o:"disha-sharma11" \
                            /d:sonar.host.url="$SONAR_HOST_URL" \
                            /d:sonar.token="$SONAR_AUTH_TOKEN" \
                            /d:sonar.test.inclusions="backend/AboriginalArtGallery.Api.Tests/**/*.cs,frontend/src/**/*.test.js" \
                            /d:sonar.exclusions="**/bin/**,**/obj/**,**/node_modules/**,frontend/build/**,backend/AboriginalArtGallery.Api/Migrations/**" \
                            /d:sonar.javascript.lcov.reportPaths="frontend/coverage/lcov.info" \
                            /d:sonar.cs.vstest.reportsPaths="artifacts/test-results/backend/*.trx"
                        
                        dotnet build backend/AboriginalArtGallery.slnx --no-restore

                        dotnet sonarscanner end /d:sonar.token="$SONAR_AUTH_TOKEN"
                    '''
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
                    check_endpoint() {
                      local environment_name="$1"
                      local url="$2"
                      local response

                      response=$(curl --silent --show-error --fail \
                        --write-out "http_code=%{http_code} time_total=%{time_total}" \
                        "$url")

                      {
                        echo "[$(date -u +"%Y-%m-%dT%H:%M:%SZ")] ${environment_name}"
                        echo "${response}"
                        echo
                      } >> monitoring-check.txt
                    }

                    : > monitoring-check.txt
                    {
                      echo "Build tag: ${IMAGE_TAG}"
                      echo "Build URL: ${BUILD_URL}"
                      echo
                    } >> monitoring-check.txt

                    check_endpoint "Staging health" "http://localhost:5142/health"
                    check_endpoint "Production health" "http://localhost:5143/health"
                '''
            }
        }
    }

    post {
        always {
            echo 'Pipeline finished.'
            archiveArtifacts artifacts: 'artifacts/**/*,frontend/coverage/lcov.info,security-dotnet.txt,security-npm.json,monitoring-check.txt', fingerprint: true
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
