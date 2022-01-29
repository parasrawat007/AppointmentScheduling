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
                },
                eventDisplay:"block",
                events: function (fetchInfo, successCallback, faliureCallback) {
                    $.ajax({
                        url: routeurl + "/api/Appointment/GetCalendarData?DoctorId=" + $("#doctorId").val(),
                        type: "GET",                       
                        dataType: "json",
                        success: function (res) {
                            var events = [];
                            if (res.status === 1 ) {
                                $.each(res.dataenum, function (i, data) {
                                    events.push({
                                        title: data.title,
                                        description: data.description,
                                        start: data.startDate,
                                        end: data.endDate,
                                        backgroundColor: data.isDoctorApproved ? "#28a745" : "#dc3545",
                                        textColor: "white",
                                        id:data.id
                                    });
                                });
                                successCallback(events);
                            }                            
                        },
                        error: function (xhr) {
                            $.notify("Error", "error");
                        }
                    });
                },
                eventClick: function (info) {
                    getEventDetailsByEventId(info.event);
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
    if (isEventDetail != null) {
        $("#id").val(obj.id);
        $("#title").val(obj.title);
        $("#description").val(obj.description);
        $("#StartDate").val(obj.startDate);
        $("#duration").val(obj.duration);
        $("#doctorId").val(obj.doctorId);
        $("#patientid").val(obj.patientId);
    }
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
                    onCloseModal();
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

function getEventDetailsByEventId(info)
{
        $.ajax({
            url: routeurl + "/api/Appointment/GetCalendarDataById/" + info.id,
            type: "GET",
            dataType: "JSON",
            success: function (res) {
                if (res.status === 1 && res.dataenum != undefined) {
                    onShowModal(res.dataenum, true);
                }
            },
            complete: function () {
                console.log('abc');
            }
        });
   
}