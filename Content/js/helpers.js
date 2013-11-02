
//Helper method for broadcasting an angular event
function broadcastAngularEvent(eventType, value) {
    var element = document.getElementById('main');
    var scope = angular.element(element).scope();
    scope.broadcastEventSafe(eventType, value);
}