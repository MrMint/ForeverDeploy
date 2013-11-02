var template;

//$(document).ready(function () {
//    template = $('#deploymentTemplate').clone();
//    template.attr('id', '');
//    $('#deploymentsContainer').prepend(template.clone());
//});

function append() {
    $('#deploymentsContainer').prepend(template.clone());
}

//Helper function for showing/hiding the log for a deployment
function showHideLog(expander, deployment) {
    //Get deployments classlist, and reverse the current state of hidelog
    var cl = deployment[0].classList;
    if (cl.contains('hideLog')) {
        cl.remove('hideLog');
    }
    else {
        cl.add('hideLog');
    }

    //Switch the direction the expander points
    cl = expander.children()[0].classList;
    if (cl.contains('fa-chevron-down')) {
        cl.remove('fa-chevron-down');
        cl.add('fa-chevron-up');
    }
    else {
        cl.remove('fa-chevron-up');
        cl.add('fa-chevron-down');
    }
}

