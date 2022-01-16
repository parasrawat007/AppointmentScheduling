$(document).ready(function () {
    InitializeCalendar();
});
function InitializeCalendar(){
    try {
        $("#calendar").fullCalendar({
            timezone: false,
            header: {
                left: "prev,next,today",
                center: "title",
                right:"month,agendaWeek,agendaDay"
            },
            selectTable: true,
            editable: false             
        });
    }
    catch (e) {
        alert(e);
    }
}