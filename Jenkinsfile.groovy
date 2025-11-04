def PROJECT_NAME = "jenkins-unity-test"
def CUSTOM_WORKSPACE = "C:\\tools\\Jenkins\\${PROJECT_NAME}"
def UNITY_VERSION = "6000.2.6f2"
def UNITY_INSTALLATION = "C:\\Program Files\\Unity\\Hub\\Editor\\${UNITY_VERSION}\\Editor"

pipeline{
    environment{
        PROJECT_PATH = "${CUSTOM_WORKSPACE}"
    }


    agent {
        any{
            customWorkspace "${CUSTOM_WORKSPACE}"
        }
    }

    stages{
        stage('Run Tests'){
            when{expression {TEST_PROJECT == 'true'}}
            steps{
                script{
                    withEnv(["UNITY_PATH=${UNITY_INSTALLATION}"]){
                        // Run EditMode tests
                        bat'''
                        "%UNITY_PATH%/Unity.exe" -batchmode -projectPath "%WORKSPACE%" -runTests -testPlatform editmode -logFile "%WORKSPACE%/test-results-editmode.log" -testResults "editmodetests.xml"
                        '''
                        
                        // Run PlayMode tests
                        bat'''
                        "%UNITY_PATH%/Unity.exe" -batchmode -projectPath "%WORKSPACE%" -runTests -testPlatform playmode -logFile "%WORKSPACE%/test-results-playmode.log" -testResults "playmodetests.xml"
                        '''
                    }
                }
            }
        }

        stage('Build Windows'){
            when{expression {BUILD_WINDOWS == 'true'}}
            steps{
                script{
                    withEnv(["UNITY_PATH=${UNITY_INSTALLATION}"]){
                        bat'''
                        "%UNITY_PATH%/Unity.exe" -quit -batchmode -projectPath "%WORKSPACE%" -executeMethod BuildScript.BuildWindows -logFile -
                        '''
                    }
                }
            }
        }

        stage('Deploy Windows'){
            when{expression {DEPLOY_WINDOWS == 'true'}}
            steps{
                echo 'Deploy Windows'
            }
        }
    }
}