<?php

// Read the database credentials from the external file
include_once( dirname(__FILE__) . '/../.config.php');

// From a mixed var, return an integer value if it is integer.
// Return null if not.
function retInt($s)
{
    if (preg_match('/^\d{1,11}$/', $s)) {
        return intval($s);
    }
    return null;
}

// Get the start and end timestamps from the GET variables
$start = retInt($_GET['start']);
$end =  retInt($_GET['end']);

// Error if no start is given
if ($start === null) {
    echo '{ "errorCode": 2, "errorDescription": "You must specify the `start` period." }';
    exit (2);
}

// Connect to the database
$conn = new mysqli($servername, $username, $password, $dbname);

// Check connection
if ($conn->connect_error) {
    echo '{ "errorCode": -1, "errorDescription": "Internal API error." }';
    die("Connection failed: " . $conn->connect_error);
}

// Define the query to get the prices between the start and end timestamps
if ($end === null) {
    // no end supplied
    $sql = "SELECT * FROM tmg_prices WHERE epoch >= ? ORDER BY epoch ASC";
    $stmt = $conn->prepare($sql);
    $stmt->bind_param('i', $start);
} else {
    $sql = "SELECT * FROM tmg_prices WHERE epoch >= ? AND epoch <= ? ORDER BY epoch ASC";
    $stmt = $conn->prepare($sql);
    $stmt->bind_param('ii', $start, $end);
}

// Execute the query
$stmt->execute();

// Get the result set
$result = $stmt->get_result();

// Loop through the result set and add the price data to the array
$prices = array();
while ($row = $result->fetch_assoc()) {
    $prices[] = $row;
}

// Encode the array as a JSON object and return it
echo json_encode($prices);

// Close the database connection
$stmt->close();
$conn->close();
