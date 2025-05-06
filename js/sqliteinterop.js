var db;

// Initialize the database
window.initSqlDb = async function (dbPath) {
    const SQL = await initSqlJs({ locateFile: file => `https://cdnjs.cloudflare.com/ajax/libs/sql.js/1.8.0/${file}` });

    // Fetch the pre-populated database file
    const response = await fetch(dbPath);
    const arrayBuffer = await response.arrayBuffer();
    const uInt8Array = new Uint8Array(arrayBuffer);

    // Open the database
    db = new SQL.Database(uInt8Array);
    console.log("SQLite database initialized");
};

// Execute queries
window.executeSqlQuery = function (query) {
    if (!db) {
        throw "Database not initialized";
    }

    const results = db.exec(query);
    console.log("Query executed:", query);
    console.log("Results:", results);
    if (results.length === 0) {
        return [];
    }

    // Convert results to array format
    let values = results[0].values.map(row => row.map(value => value !== null ? value.toString() : ""));
    console.log(values);
    return values;
};

window.getColumnNames = function (tableName) {
    if (!db) {
        throw "Database not initialized";
    }
    const query = "SELECT name FROM pragma_table_info('" + tableName + "');";
    const results = db.exec(query);
    console.log("Query executed:", query);
    console.log("Results:", results);

    if (results.length === 0) {
        return [];
    }

    // Extract just the column names as a simple string array
    const columnNames = results[0].values;
    console.log("Column names:", columnNames);
    return columnNames;
};

// Add this to allow saving changes to the database
window.exportDatabaseToFile = function () {
    if (!db) {
        throw "Database not initialized";
    }

    // Export the database to a Uint8Array
    const data = db.export();

    // Create a blob and download it
    const blob = new Blob([data], { type: "application/octet-stream" });
    const url = URL.createObjectURL(blob);

    // Create a link and trigger the download
    const a = document.createElement("a");
    a.href = url;
    a.download = "PMGChronical.db";
    a.click();

    // Clean up
    URL.revokeObjectURL(url);

    return true;
};

// Create a new competition
window.createCompetition = function (competition) {
    if (!db) {
        throw "Database not initialized";
    }

    const query = `
        INSERT INTO Competitions (id, name, description, subject, period_from, period_to, tag, school_year)
        VALUES (?, ?, ?, ?, ?, ?, ?, ?);
    `;
    const stmt = db.prepare(query);
    stmt.run([
        competition.id,
        competition.name,
        competition.description,
        competition.subject,
        competition.period_from,
        competition.period_to,
        competition.tag,
        competition.school_year
    ]);
    stmt.free();
    console.log("Competition created:", competition);
};

// Read all competitions
window.readCompetitions = function () {
    if (!db) {
        throw "Database not initialized";
    }

    const query = "SELECT * FROM Competitions;";
    const results = db.exec(query);
    console.log("Competitions read:", results);

    if (results.length === 0) {
        return [];
    }

    return results[0].values.map(row => row.map(value => value !== null ? value.toString() : ""));
};

// Update a competition
window.updateCompetition = function (competition) {
    if (!db) {
        throw "Database not initialized";
    }

    const query = `
        UPDATE Competitions
        SET name = ?, description = ?, subject = ?, period_from = ?, period_to = ?, tag = ?, school_year = ?
        WHERE id = ?;
    `;
    const stmt = db.prepare(query);
    stmt.run([
        competition.name,
        competition.description,
        competition.subject,
        competition.period_from,
        competition.period_to,
        competition.tag,
        competition.school_year,
        competition.id
    ]);
    stmt.free();
    console.log("Competition updated:", competition);
};

// Delete a competition
window.deleteCompetition = function (id) {
    if (!db) {
        throw "Database not initialized";
    }

    const query = "DELETE FROM Competitions WHERE id = ?;";
    const stmt = db.prepare(query);
    stmt.run([id]);
    stmt.free();
    console.log("Competition deleted with id:", id);
};

window.showModal = (modalId) => {
    const modalElement = document.getElementById(modalId);
    const modal = new bootstrap.Modal(modalElement);
    modal.show();
    return true;
};

window.hideModal = (modalId) => {
    const modalElement = document.getElementById(modalId);
    const modal = bootstrap.Modal.getInstance(modalElement);
    if (modal) {
        modal.hide();
        return true;
    }
    return false;
};
