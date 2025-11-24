let dotNetHelper = null;
let pingInterval = null;

export function initializeConnectionMonitor(dotNetReference) {
    dotNetHelper = dotNetReference;
    
    // Check immediately
    checkInternetConnection();
    
    // Check status every 10 seconds
    pingInterval = setInterval(checkInternetConnection, 10000);
}

async function checkInternetConnection() {
    try {
        // Ping Google's favicon (very small, fast, reliable)
        const controller = new AbortController();
        const timeoutId = setTimeout(() => controller.abort(), 3000); // 3 second timeout
        
        const response = await fetch('https://www.google.com/favicon.ico', {
            mode: 'no-cors', // Important: allows cross-origin without CORS issues
            cache: 'no-cache',
            signal: controller.signal
        });
        
        clearTimeout(timeoutId);
        updateConnectionStatus(true);
    } catch (error) {
        updateConnectionStatus(false);
    }
}

function updateConnectionStatus(isOnline) {
    if (dotNetHelper) {
        dotNetHelper.invokeMethodAsync('UpdateConnectionStatus', isOnline);
    }
}

export function cleanupConnectionMonitor() {
    if (pingInterval) {
        clearInterval(pingInterval);
    }
    dotNetHelper = null;
}