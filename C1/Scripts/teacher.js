function AddTeacher() {

    //goal: send a request 
    //POST : http://localhost:51326/api/TeacherData/AddTeacher/
    //with POST data of authorname, bio, email, etc.
    console.log("here");
    var URL = "/api/TeacherData/AddTeacher/";

    var rq = new XMLHttpRequest();

    //entering user datae
    var TeacherFname = document.getElementById('TeacherFname').value;
    var TeacherLname = document.getElementById('TeacherLname').value;
    var Employeenumber = document.getElementById('Employeenumber').value;
    var Hiredate = document.getElementById('Hiredate').value;
    var Salary = document.getElementById('Salary').value;
    var messageresponse = document.getElementById('response');

    var TeacherData = {
        "TeacherFname": TeacherFname,
        "TeacherLname": TeacherLname,
        "Employeenumber": Employeenumber,
        "Hiredate": Hiredate,
        "Salary": Salary
    };


    rq.open("POST", URL, true);
    rq.setRequestHeader("Content-Type", "application/json");
    rq.onreadystatechange = function () {
        console.log(rq.status)
        //ready state should be 4 AND status should be 200
        if (rq.readyState == 4 && rq.status == 204) {
            //request is successful and the request is finished
            console.log("In success")
            messageresponse.innerHTML = "successfully added teacher";


        }
        else {
            console.log("In fail");
            messageresponse.innerHtml = "unsuccessful";
        }

    }
    //POST information sent through the .send() method
    rq.send(JSON.stringify(TeacherData));

};

function DeleteTeacher(Teacherid){
    //goal: send a request
    //POST : http://localhost:51326/api/TeacherData/DeleteTeacher/
    //with data of Id.
    console.log("here");
    var URL = "https://localhost:44393/api/TeacherData/DeleteTeacher/" + Teacherid;

    var rq = new XMLHttpRequest();

    var TeacherData = {
        "id": Teacherid
    };


    rq.open("POST", URL, true);
    rq.setRequestHeader("Content-Type", "application/json");
    rq.onreadystatechange = function () {
        console.log(rq.status)
        //ready state should be 4 AND status should be 204
        if (rq.readyState == 4 && rq.status == 204) {
            //request is successful and the request is finished
            console.log("In success")
            window.location.replace("/Teacher/List")
        }

    }
    //POST information sent through the .send() method
    rq.send();
}