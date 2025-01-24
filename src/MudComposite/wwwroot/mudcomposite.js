function goBack() {
    window.history.back();
}

function goForward() {
    window.history.forward();
}

function copyToClipboard(text) {
    window.copyToClipboard = (text) => {
        navigator.clipboard.writeText(text).then(
            () => console.log("Copied to clipboard"),
            (err) => console.error("Failed to copy text to clipboard", err)
        );
    };
}