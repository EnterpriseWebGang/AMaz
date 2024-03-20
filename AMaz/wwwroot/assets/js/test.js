
const coursesData = [
    { id: 1, courseName: 'Course 1', topic: 'Topic 1', instructor: 'Instructor 1' },
    { id: 2, courseName: 'Course 2', topic: 'Topic 2', instructor: 'Instructor 2' },
    // Add more courses as needed
];

// Function to simulate searching for courses
function searchCourses() {
    const courseName = $('#courseName').val().toLowerCase();
    const topic = $('#topic').val().toLowerCase();
    const instructor = $('#instructor').val().toLowerCase();

    // Hide the registration section
    $('#registration').hide();

    // Filter courses based on search criteria
    const results = coursesData.filter(course =>
        course.courseName.toLowerCase().includes(courseName) &&
        course.topic.toLowerCase().includes(topic) &&
        course.instructor.toLowerCase().includes(instructor)
    );

    // Display search results
    displaySearchResults(results);
}

// Function to display search results
function displaySearchResults(results) {
    const searchResultsContent = $('#searchResultsContent');

    // Build HTML for search results
    let html = '<h3>Search Results</h3>';
    if (results.length > 0) {
        html += '<ul>';
        results.forEach(course => {
            html += `<li><a href="#" onclick="viewSubjectDetails(${course.id})">${course.courseName}</a></li>`;
        });
        html += '</ul>';
    } else {
        html += '<p>No results found.</p>';
    }

    // Display search results content
    searchResultsContent.html(html);
    $('#searchForm').hide();
    $('#searchResults').show();
}

// Function to view subject details
function viewSubjectDetails(courseId) {
    const subjectDetailsContent = $('#subjectDetailsContent');

    // Retrieve subject details based on courseId (you may fetch this data from a server)
    const subjectDetails = coursesData.find(course => course.id === courseId);

    // Build HTML for subject details
    const html = `
        <h3>Subject Details</h3>
        <p><strong>Course Name:</strong> ${subjectDetails.courseName}</p>
        <p><strong>Topic:</strong> ${subjectDetails.topic}</p>
        <p><strong>Instructor:</strong> ${subjectDetails.instructor}</p>
        <div class="btn">
            <a href="#" onclick="submitWork(${subjectDetails.id})">Submit Work</a>
        </div>
    `;

    // Display subject details content
    subjectDetailsContent.html(html);
    $('#searchResults').hide();
    $('#subjectDetails').show();
}

// Function to simulate work submission
function submitWork(courseId) {
    const workSubmissionContent = $('#workSubmissionContent');

    // Retrieve subject details based on courseId (you may fetch this data from a server)
    const subjectDetails = coursesData.find(course => course.id === courseId);

    // Build HTML for work submission form
    const html = `
        <h3>Work Submission</h3>
        <p><strong>Course Name:</strong> ${subjectDetails.courseName}</p>
        <p><strong>Topic:</strong> ${subjectDetails.topic}</p>
        <p><strong>Instructor:</strong> ${subjectDetails.instructor}</p>
        <form action="/submit-work" method="post" enctype="multipart/form-data">
            <!-- Add form fields for work submission (e.g., file input, image upload) -->
            <input type="file" name="wordFile" accept=".doc, .docx">
            <input type="file" name="imageFile" accept="image/*">
            <div class="btn">
                <button type="submit">Submit</button>
            </div>
        </form>
    `;

    // Display work submission form content
    workSubmissionContent.html(html);
    $('#subjectDetails').hide();
    $('#workSubmission').show();
}
