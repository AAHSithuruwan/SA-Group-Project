// Function to store the JWT token
export const storeJwtToken = async (token) => {
    if (token) {
        localStorage.setItem('jwtToken', token); // Store the Jwt token in local storage
        console.log("Jwt Token stored successfully");
    } else {
        console.error("No Jwt token provided to store");
    }
};

export const getJwtToken = async () => {
    return localStorage.getItem('jwtToken'); // Retrieve the JWT token
};

export const removeJwtToken = async () => {
    localStorage.removeItem('jwtToken'); // Remove the Jwt token
    console.log("Jwt Token removed successfully");
};