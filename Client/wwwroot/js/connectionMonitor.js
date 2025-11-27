let dotNetHelper = null;
let pingInterval = null;

export function initializeConnectionMonitor(dotNetReference) {
    console.log("Connection monitor initialized");
    dotNetHelper = dotNetReference;
    
    // Check immediately
    checkInternetConnection();
    
    // Check status every 5 seconds
    pingInterval = setInterval(checkInternetConnection, 5000);
}

async function checkInternetConnection() {
    try {
        const controller = new AbortController();
        const timeoutId = setTimeout(() => controller.abort(), 3000);
        
        const response = await fetch('https://www.google.com/favicon.ico', {
            mode: 'no-cors',
            cache: 'no-cache',
            signal: controller.signal
        });
        
        clearTimeout(timeoutId);
        updateConnectionStatus(true);
    } catch (error) {
        console.log("Offline detected:", error.message);
        updateConnectionStatus(false);
    }
}

function updateConnectionStatus(isOnline) {
    if (dotNetHelper) {
        dotNetHelper.invokeMethodAsync('UpdateConnectionStatus', isOnline)
            .catch(err => console.error("Error calling UpdateConnectionStatus:", err));
    } else {
        console.error("dotNetHelper is null");
    }
}

export function cleanupConnectionMonitor() {
    if (pingInterval) {
        clearInterval(pingInterval);
    }
    dotNetHelper = null;
}