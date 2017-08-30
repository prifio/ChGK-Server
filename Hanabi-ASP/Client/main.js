$.post('../api/main', {
    "X": 10,
    "Y": 20,
    "IsEmpty": false
}, function () {
    console.log(10);
}, 'json');