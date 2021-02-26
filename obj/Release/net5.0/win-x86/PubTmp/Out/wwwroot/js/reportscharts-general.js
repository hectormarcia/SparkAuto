var canvas1 = document.getElementById("Canvas1");
var canvas2 = document.getElementById("Canvas2");
var canvas3 = document.getElementById("Canvas3");
var canvas4 = document.getElementById("Canvas4");
var canvas5 = document.getElementById("Canvas5");

new Chart(canvas1, {
    type: getInputValue("chart1_type"),
    data: {
        labels: getInputValue("chart1_labels"),
        datasets: [{
            label: getInputValue("chart1_axis"),
            data: getInputValue("chart1_values"),
            backgroundColor: chartbgcolors(nvals("chart1_values"))
        }]
    },
    options: {
        scales: {
            yAxes: [{
                display: true,
                ticks: {
                    suggestedMin: 0,
                    beginAtZero: true
                }
            }]
        }
    }
});

new Chart(canvas4, {
    type: getInputValue("chart4_type"),
    data: {
        labels: getInputValue("chart4_labels"),
        datasets: [{
            label: getInputValue("chart4_axis"),
            data: getInputValue("chart4_values"),
            backgroundColor: chartbgcolors(nvals("chart4_values"))
        }]
    },
    options: {
        scales: {
            yAxes: [{
                display: true,
                ticks: {
                    suggestedMin: 0,
                    beginAtZero: true
                }
            }]
        }
    }
});

new Chart(canvas5, {
    type: getInputValue("chart5_type"),
    data: {
        labels: getInputValue("chart5_labels"),
        datasets: [{
            label: getInputValue("chart5_axis"),
            data: getInputValue("chart5_values"),
            backgroundColor: chartbgcolors(nvals("chart5_values"))
        }]
    },
    options: {
        scales: {
            yAxes: [{
                display: true,
                ticks: {
                    suggestedMin: 0,
                    beginAtZero: true
                }
            }]
        }
    }
});

new Chart(canvas2,{
    type: 'bar',
    data: {
        labels: getInputValue("facservices_labels"),
        datasets: [{
            label: "Services purchased by the customer",
            borderColor: 'rgb(102,51,153)',
            backgroundColor: "rgba(102,51,153,0.6)",
            data: getInputValue("facservices_values")
        }]
    },
    options: {
        scales: {
            yAxes: [{
                display: true,
                ticks: {
                    suggestedMin: 0,
                    beginAtZero: true
                }
            }]
        }
    }
});


new Chart(canvas3, {
    type: 'line',
    data: {
        labels: getInputValue("facturas_labels"),
        datasets: [{
            label: "Timeline",
            data: getInputValue("facturas_values"),
            fill: false,
            pointRadius: 5,
            pointHoverRadius: 10,
            showLine: true,
            borderColor: chartbgcolors(1)
        }]
    },
    options:{
        elements: {
            line: {
                tension: 0
            }
        }
    }
});




/*AUXILIARS*/

function getInputValue(id){
    return document.getElementById(id).value.toString().split(",");
}

function nvals(id){
    return getInputValue(id).length;
}

function chartbgcolors(n){
    var bg = [];
    for(var i = 0; i < n; i++){
        var r = Math.floor((Math.random()*255)+1);
        var g = Math.floor((Math.random()*255)+1);
        var b = Math.floor((Math.random()*255)+1);
        bg.push('rgba(' + r + "," + g + "," + b + ",0.8)");
    }
    return bg;
}

