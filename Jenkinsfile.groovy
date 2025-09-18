def PROJECT_NAME = "jenkins-unity-test"
def CUSTOM_WORKSPACE = "C:\\tools\\Jenkins\\${PROJECT_NAME}"
def UNITY_VERSION = "6000.2.1f1"
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
            when{expression {BUILD_WINDOWS == 'true'}}
            steps{
                echo 'Deploy Windows'
            }
        }
    }
}