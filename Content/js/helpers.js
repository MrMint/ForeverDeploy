var noDeployments;
var pOSTReceived;
var deserializing;
var deserializingFailed;
var processingCommits;
var processingCommitsFailed;
var openingRepo;
var updatingRepo;
var updatingRepoFailed;
var building;
var buildingFailed;
var starting;
var deployed;
var statusBar;

//Helper method for broadcasting an angular event
function broadcastAngularEvent(eventType, value) {
    var element = document.getElementById('main');
    var scope = angular.element(element).scope();
    scope.broadcastEventSafe(eventType, value);
}

//Setup function for displaying dome elements
function initialize() {

    statusBar = {
        0:'deploying',
        1:'deploying',
        2:'deploying',
        3: 'failed',
        4:'deploying',
        5: 'failed',
        6:'deploying',
        7:'deploying',
        8: 'failed',
        9:'deploying',
        10:'failed',
        11:'deploying',
        12: 'deployed',
        null: 'deploying',
        'undefined': 'deploying'
    }

    var deploying = ["deploying"];
    var deploymentFailed = ["deploymentFailed"];
    var deploySuccess = ["deployed"];

    //----LOGS----
    //Preparing
    var logPreparingInProgress = ["logPreparing", "logPreparingInProgress"];
    var logPreparingSuccess = ["logPreparing", "logPreparingSuccess"];
    var logPreparingFailed = ["logPreparing", "logPreparingFailed"];

    //Updating
    var logUpdatingInProgress = ["logUpdating", "logUpdatingInProgress"];
    var logUpdatingSuccess = ["logUpdating", "logUpdatingSuccess"];
    var logUpdatingFailed = ["logUpdating", "logUpdatingFailed"];

    //Building
    var logBuildingInProgress = ["logBuilding", "logBuildingInProgress"];
    var logBuildingSuccess = ["logBuilding", "logBuildingSuccess"];
    var logBuildingFailed = ["logBuilding", "logBuildingFailed"];

    //Deploying
    var logDeployingInProgress = ["logDeploying", "logDeployingInProgress"];
    var logDeployingSuccess = ["logDeploying", "logDeployingSuccess"];
    var logDeployingFailed = ["logDeploying", "logDeployingFailed"];
    //----LOGS-END----

    
    //TODO: Change to hash maps for quicker access times
    //Build the arrays
    noDeployments = ["noDeployments"];
    pOSTReceived = deploying.concat(logPreparingInProgress);
    deserializing = deploying.concat(logPreparingInProgress);
    deserializingFailed = deploymentFailed.concat(logPreparingFailed);
    processingCommits = deploying.concat(logPreparingInProgress);
    processingCommitsFailed = deploymentFailed.concat(logPreparingFailed);
    openingRepo = deploying.concat(logUpdatingInProgress).concat(logPreparingSuccess);
    updatingRepo = deploying.concat(logUpdatingInProgress).concat(logPreparingSuccess);
    updatingRepoFailed = deploymentFailed.concat(logUpdatingFailed).concat(logPreparingSuccess);
    building = deploying.concat(logBuildingInProgress).concat(logUpdatingSuccess).concat(logPreparingSuccess);
    buildingFailed = deploymentFailed.concat(logBuildingFailed).concat(logUpdatingSuccess).concat(logPreparingSuccess);
    starting = deploying.concat(logBuildingSuccess).concat(logUpdatingSuccess).concat(logPreparingSuccess);
    deployed = deploySuccess.concat(logDeployingSuccess).concat(logBuildingSuccess).concat(logUpdatingSuccess).concat(logPreparingSuccess);
}

//Helper method for determining DOM elements visibility based on deployment status
function showElement(name, status) {


    switch (status) {
        //NoDeployments
        case 0:
            return showElementHelper(name, noDeployments);

        //POSTReceived
        case 1:
            return showElementHelper(name, pOSTReceived);

        //Deserializing
        case 2:
            return showElementHelper(name, deserializing);

        //DeserializingFailed
        case 3:
            return showElementHelper(name, deserializingFailed);

        //ProcessingCommits
        case 4:
            return showElementHelper(name, processingCommits);

        //ProcessingCommitsFailed
        case 5:
            return showElementHelper(name, processingCommitsFailed);

        //OpeningRepo
        case 6:
            return showElementHelper(name, openingRepo);

        //UpdatingRepo
        case 7:
            return showElementHelper(name, updatingRepo);

        //UpdatingRepoFailed
        case 8:
            return showElementHelper(name, updatingRepoFailed);

        //Building
        case 9:
            return showElementHelper(name, building);

        //BuildingFailed
        case 10:
            return showElementHelper(name, buildingFailed);

        //Starting
        case 11:
            return showElementHelper(name, starting);

        //Deployed
        case 12:
            return showElementHelper(name, deployed);
        default:
            return false;
    }
}

//Helper function that performs the showElement check
function showElementHelper(name, acceptableElements) {
    if ($.inArray(name, acceptableElements) >=0) {
        return true;
    }
    else {
        return false;
    }
}

