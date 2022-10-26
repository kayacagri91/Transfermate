function UserAuth() {
    var formdata = new FormData();
    formdata.append("Username", $("#username").val());
    formdata.append("Password", $("#password").val());

    $.ajax({
        url: '/Home/JsonLogin',
        type: 'POST',
        data: formdata,
        dataType: 'json',
        contentType: false,
        processData: false,
        success: function (data) {
            if (!data.IsSuccess) {
                $("#Error-Label").removeClass("label-success");
                $("#Error-Label").addClass("label-danger");
                $("#Error-Label").html(data.Message);
            }
            else {
                $("#Error-Label").removeClass("label-danger");
                $("#Error-Label").addClass("label-success");
                $("#Error-Label").html(data.Message);
                if (data.Url != null || data.Url != "") {
                    window.location.href = data.Url;
                }
            }
        },
        error: function (response) {
            alert(response.responseText);
        }
    });
}

function CreateTask() {
    var formdata = new FormData();
    formdata.append("TaskName", $("#taskname").val());
    formdata.append("Description", $("#description").val());
    formdata.append("StartDate", $("#baslangictarih").val());
    formdata.append("EndDate", $("#bitistarih").val());
    formdata.append("Status", $("#Status").val());

    $.ajax({
        url: '/Home/JsonTodoCreate',
        type: 'POST',
        data: formdata,
        dataType: 'json',
        contentType: false,
        processData: false,
        success: function (data) {
            if (!data.IsSuccess) {
                $("#CreateError").removeClass("label-success");
                $("#CreateError").addClass("label-danger");
                $("#CreateError").html(data.Message);
            }
            else {
                $("#CreateError").removeClass("label-danger");
                $("#CreateError").addClass("label-success");
                $("#CreateError").html(data.Message);
                if (data.Url != null || data.Url != "") {
                    window.location.href = data.Url;
                }
            }
        },
        error: function (response) {
            alert(response.responseText);
        }
    });
}

function SearchTask() {
    var taskname = $("#tasknamesearch").val();
    var startdate = $("#baslangictarihsearch").val();
    var enddate = $("#bitistarihsearch").val();
    var status = $("#Statussearch").val();
    window.location.href = '/Home/TodoList?Taskname=' + taskname + '&Startdate=' + startdate + '&Enddate=' + enddate + '&Status=' + status;
}

function GetUpdateInformation(id, Taskname, Description, StartDate, EndDate, Status) {
    $("#tasknameupdate").val(Taskname);
    $("#taskid").val(id);
    $("#descriptionupdate").val(Description);
    $("#baslangictarihupdate").val(StartDate);
    $("#bitistarihupdate").val(EndDate);
    $("#Statusupdate").val(Status);
}

function UpdateTask() {
    var formdata = new FormData();
    formdata.append("id", $("#taskid").val());
    formdata.append("TaskName", $("#tasknameupdate").val());
    formdata.append("Description", $("#descriptionupdate").val());
    formdata.append("StartDate", $("#baslangictarihupdate").val());
    formdata.append("EndDate", $("#bitistarihupdate").val());
    formdata.append("Status", $("#Statusupdate").val());

    $.ajax({
        url: '/Home/JsonTodoUpdate',
        type: 'POST',
        data: formdata,
        dataType: 'json',
        contentType: false,
        processData: false,
        success: function (data) {
            if (!data.IsSuccess) {
                $("#UpdateError").removeClass("label-success");
                $("#UpdateError").addClass("label-danger");
                $("#UpdateError").html(data.Message);
            }
            else {
                $("#UpdateError").removeClass("label-danger");
                $("#UpdateError").addClass("label-success");
                $("#UpdateError").html(data.Message);
                if (data.Url != null || data.Url != "") {
                    window.location.href = data.Url;
                }
            }
        },
        error: function (response) {
            alert(response.responseText);
        }
    });
}

function DeleteTask(id) {
    var formdata = new FormData();
    formdata.append("id", id);
    $.ajax({
        url: '/Home/JsonTodoDelete',
        type: 'POST',
        data: formdata,
        dataType: 'json',
        contentType: false,
        processData: false,
        success: function (data) {
            window.location.href = data.Url;
        },
        error: function (response) {
            alert(response.responseText);
        }
    });
}