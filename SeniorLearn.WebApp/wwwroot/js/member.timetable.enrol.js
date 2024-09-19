
$(() => {
    const professionalId = parseInt($('#ProfessionalId').val());
    const lessonsTable = $('#lessonsTable tbody');

    // Function to fetch lessons from the API
    async function fetchLessons(start, end) {
        try {
            const response = await fetch(`/api/timetable/lessons?start=${encodeURIComponent(start)}&end=${encodeURIComponent(end)}`);
            if (!response.ok) {
                throw new Error(`HTTP error! Status: ${response.status}`);
            }
            return await response.json();
        } catch (error) {
            console.error('Error loading lessons:', error);
            return [];
        }
    }


    //confirm delete
    window.confirmAndDelete = function (lessonId) {
        if (confirm(`Are you sure you want to delete lesson ${lessonId}?`)) {
            deleteLesson(lessonId);
        }
    };

    // Function to render lessons in the table
    async function renderLessons() {
        const now = new Date();
        //const start = new Date(now.getFullYear(), now.getMonth(), 1).toISOString();
        const start = now.toISOString();
        const end = new Date(now.getFullYear(), now.getMonth() + 2, now.getDate()).toISOString();



        //const lessons = await fetchLessons(start, end);
        const [lessons, enrolments] = await Promise.all([fetchLessons(start, end), fetchEnrolments(start, end)]);
        lessonsTable.empty();
        lessons.forEach(lesson => {
            //const end = new Date(lesson.end).toISOString();
            const professional = lesson.professionalId === professionalId;
            const isEnrolled = enrolments.some(e => e.lessonId === lesson.id);
            const row = `
                                        <tr data-lesson-id="${lesson.id}">
                                            <td>${lesson.title}</td>
                                            <td>${lesson.description}</td>
                                            <td>${new Date(lesson.start).toLocaleString()}</td>
                                            <td>${new Date(lesson.end).toLocaleString()}</td>
                                            <td>${lesson.classDurationInMinutes}</td>
                                            <td>${lesson.isCourse ? "Yes" : "No"}</td>
                                            <td>
                                                       ${professional ? '' : isEnrolled ? `<button type="button" class="btn btn-secondary disabled">Enrolled</button>` : ` <button type="button" class="btn btn-primary" onclick="openEnrolModal(${lesson.id})">Enrol</button>`}
                                            </td>
                                            <td>
                                            ${professional ? ` <button type="button" class="btn btn-danger" onclick="confirmAndDelete(${lesson.id})">Delete</button>` : ''}
                                               
                                            </td>
                                        </tr>
                                    `;
            lessonsTable.append(row);
        });
    }

    async function fetchEnrolments(start, end) {
        try {
            const response = await fetch(`/api/member/enrolments?start=${start}&end=${end}`);
            if (response.ok) {
                return await response.json();
            } else {
                console.error("Error fetching enrolments");
                return [];
            }
        } catch (err) {
            console.error("Errot fetching enrolments: ", err);
            return [];
        }
    }



    // Function to handle enrolling in a lesson
    // Open enrol modal and display lesson details
    async function openEnrolModal(lessonId) {
        //const start = new Date().toISOString();
        //const end = new Date().toISOString();
        const now = new Date();
        //const start = new Date(now.getFullYear(), now.getMonth(), 1).toISOString();
        //const end = new Date(now.getFullYear(), now.getMonth() + 1, 0).toISOString();
        const start = now.toISOString();
        const end = new Date(now.getFullYear(), now.getMonth() + 2, now.getDate()).toISOString();

        const lessons = await fetchLessons(start, end);
        const enrolments = await fetchEnrolments(start, end);

        const lesson = lessons.find(l => l.id == lessonId);
        const isEnrolled = enrolments.some(e => e.lessonId == lessonId);


        if (lesson) {
            if (lesson.professionalId === professionalId) {
                alert("A Professional cannot enrol in their own lesson.");
                return;
            }

            $('#enrolModal').find('.modal-title').html(lesson.title);
            $('#lessonDetails').html(`
                                <p>Lesson Name: ${lesson.title}</p>
                                <p>Description: ${lesson.description}</p>
                                <p>Start Time: ${new Date(lesson.start).toLocaleString()}</p>
                                <p>End Time: ${new Date(lesson.end).toLocaleString()}</p>
                                <p>IsCourse: ${lesson.isCourse}</p>
                            `);

            if (isEnrolled) {
                $('#lessonDetails').append('<p>Alert: You are already enrolled in this lesson :)</p>');
                $('#enrol-submit').hide();
            } else {
                const message = lesson.isCourse
                    ? "Note: This lesson belongs to a course and you will automatically be enrolled in all lessons that are part of the course. You are also expected to attend all lessons for the course"
                    : "";

                $('#lessonDetails').append(`<p>${message}</p>`);
                $('#enrol-submit').show().off('click').on('click', () => enrolInLesson(lessonId));
            }

            $('#enrolModal').modal('show');
        } else {
            console.error('Lesson not found');
        }
    }

    // Enrol in lesson
    async function enrolInLesson(lessonId, isCourse) {
        try {
            const response = await fetch(`/api/timetable/lessons/${lessonId}/enrol`, { method: 'POST' });

            const contentType = response.headers.get('content-type');
            let result;

            if (contentType && contentType.indexOf('application/json') !== -1) {
                result = await response.json();
            } else {
                result = await response.text();
            }
            //const result = await response.json();

            if (response.ok) {
                if (isCourse) {
                    alert(`Error from enrolLessons function: You have enrolled in the lesson ${isCourse ? " and You are also expected to attend all lessons for the course." : "."}`)
                } else {
                    alert("You have enrolled in the lesson");
                };
                $('#enrolModal').modal('hide');
                renderLessons();
            } else {
                alert(result.message || ' enrolLessons function:You have already enrolled in this lesson! ');
                $('#enrolModal').modal('hide');
            }
        } catch (error) {
            console.error('Error enrolling in lesson:', error);
        }
    }

    //Delete lesson
    async function deleteLesson(lessonId) {
        try {
            const response = await fetch(`/api/timetable/lessons/${lessonId}`, {
                method: 'DELETE',
                headers: {
                    'Content-Type': 'application/json'
                }
            });

            const contentType = response.headers.get('content-type');
            let data;

            if (contentType && contentType.includes('application/json')) {
                data = await response.json();
            } else
            {
                data = await response.text();
            }

            if (!response.ok) {
                throw new Error(data.message || 'Failed to delete lesson, only lesson in Draft status can be deleted.');
            }
         

            alert(data.message);

            const row = document.querySelector(`tr[data-lesson-id='${lessonId}']`);
            if (row) {
                row.remove();
            }

        } catch (error) {
            console.error("Error deleting lesson", error);
            alert(`Error: ${error.message}`)
        }
    }



    // Initial render of lessons
    renderLessons();

    // Close modal handler
    $('#enrol-close').on('click', () => $('#enrolModal').modal('hide'));

    // Expose function to global scope
    window.openEnrolModal = openEnrolModal;
});
