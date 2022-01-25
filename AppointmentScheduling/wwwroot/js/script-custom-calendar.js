var routeurl = location.protocol + "//"+location.host;
$(document).ready(function () {
    InitializeCalendar();
    $('#StartDate').kendoDateTimePicker({
        value: new Date(),
        dateInput: false
    });
});
function InitializeCalendar(){
    try {        
        var calendarEl = document.getElementById('calendar');
        if (calendarEl != null)
        {
            var calendar = new FullCalendar.Calendar(calendarEl, {
                initialView: 'dayGridMonth',
                headerToolbar: {
                    left: "prev,next,today",
                    center: "title",
                    right: "dayGridMonth,timeGridWeek,timeGridDay"
                },
                selectable: true,
                editable: false,
                select: function (event) {
                    onShowModal(event, null);
                }
            });
            calendar.render();
        }

        //$("#calendar").fullCalendar({
        //    timezone: false,
        //    header: {
        //        left: "prev,next,today",
        //        center: "title",
        //        right:"month,agendaWeek,agendaDay"
        //    },
        //    selectable: true,
        //    editable: false,
        //    select: function (event) {
        //        onShowModal(event, null);
        //    }
        //});
    }
    catch (e) {
        alert(e);
    }
}

function onShowModal(obj, isEventDetail)
{
    $('#AppointmentInput').modal("show");
}

function onCloseModal() {
    $('#AppointmentInput').modal("hide");
}
function onSubmitForm() {
    if (checkValidation()) {
        var requestData =
        {
            Id: parseInt($("#id").val()),
            Title: $("#title").val(),
            Description: $("#description").val(),
            StartDate: $("#StartDate").val(),
            Duration: $("#duration").val(),
            DoctorId: $("#doctorId").val(),
            PatientId: $("#patientid").val(),
        };
        $.ajax({
            url: routeurl + "/api/Appointment/SaveCalendarData",
            type: "POST",
            data: JSON.stringify(requestData),
            contentType: "application/json",
            success: function (res) {
                if (res.status === 1 || res.status === 2) {
                    $.notify(res.message, "success");
                } else {
                    $.notify(res.message, "error");
                }
            },
            error: function (xhr) {
                $.notify("Error", "error");
            }
        });
    }
}
function checkValidation()
{
    var isValid = true;

    if ($("#title").val() === undefined || $("#title").val() === "") {
        isValid = false;
        $("#title").addClass('error');
    }
    else {
        $("#title").removeClass('error');
    }

    if ($("#StartDate").val() === undefined || $("#StartDate").val() === "") {
        isValid = false;
        $("#StartDate").addClass('error');
    }
    else {
        $("#StartDate").removeClass('error');
    }
    return isValid;
}