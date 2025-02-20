import React, { useState } from "react";
import { Send, AlertCircle, CheckCircle } from "lucide-react";

const MessageDashboard = () => {
  const [message, setMessage] = useState("");
  const [messages, setMessages] = useState([]);
  const [status, setStatus] = useState(null);
  const [loading, setLoading] = useState(false);

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (!message.trim()) return;

    setLoading(true);
    try {
      const response = await fetch("http://localhost:5000/api/message", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({
          content: message,
          timestamp: new Date().toISOString(),
        }),
      });

      if (!response.ok) throw new Error("Failed to send message");

      setMessages((prev) => [
        {
          id: Date.now(),
          content: message,
          timestamp: new Date().toISOString(),
          status: "Sent",
        },
        ...prev,
      ]);

      setMessage("");
      setStatus({ type: "success", message: "Message sent successfully!" });
    } catch (error) {
      setStatus({
        type: "error",
        message: "Failed to send message. Please try again.",
      });
    } finally {
      setLoading(false);
    }
  };

  return (
    <div
      style={{
        minHeight: "100vh",
        backgroundColor: "#f9fafb",
        padding: "2rem",
      }}
    >
      <div style={{ maxWidth: "64rem", margin: "0 auto" }}>
        <div
          style={{
            backgroundColor: "white",
            borderRadius: "0.5rem",
            padding: "1.5rem",
            boxShadow: "0 1px 2px rgba(0, 0, 0, 0.05)",
          }}
        >
          <h1
            style={{
              fontSize: "1.5rem",
              fontWeight: "bold",
              marginBottom: "1.5rem",
              color: "#1f2937",
            }}
          >
            Message Queue Dashboard
          </h1>

          <form onSubmit={handleSubmit} style={{ marginBottom: "1.5rem" }}>
            <div style={{ display: "flex", gap: "1rem" }}>
              <input
                type="text"
                value={message}
                onChange={(e) => setMessage(e.target.value)}
                placeholder="Type your message..."
                style={{
                  flex: 1,
                  padding: "0.75rem",
                  border: "1px solid #e5e7eb",
                  borderRadius: "0.5rem",
                  outline: "none",
                }}
              />
              <button
                type="submit"
                disabled={loading || !message.trim()}
                style={{
                  padding: "0.75rem 1.5rem",
                  backgroundColor: "#2563eb",
                  color: "white",
                  borderRadius: "0.5rem",
                  display: "flex",
                  alignItems: "center",
                  gap: "0.5rem",
                  cursor:
                    loading || !message.trim() ? "not-allowed" : "pointer",
                  opacity: loading || !message.trim() ? 0.5 : 1,
                }}
              >
                <Send size={20} />
                Send
              </button>
            </div>
          </form>

          {status && (
            <div
              style={{
                marginBottom: "1.5rem",
                padding: "1rem",
                borderRadius: "0.5rem",
                backgroundColor:
                  status.type === "success" ? "#f0fdf4" : "#fef2f2",
                color: status.type === "success" ? "#15803d" : "#dc2626",
                display: "flex",
                alignItems: "center",
                gap: "0.5rem",
              }}
            >
              {status.type === "success" ? (
                <CheckCircle size={20} />
              ) : (
                <AlertCircle size={20} />
              )}
              {status.message}
            </div>
          )}

          <div style={{ marginTop: "1.5rem" }}>
            <h2
              style={{
                fontSize: "1.125rem",
                fontWeight: 600,
                color: "#374151",
                marginBottom: "1rem",
              }}
            >
              Recent Messages
            </h2>
            {messages.length === 0 ? (
              <p
                style={{
                  textAlign: "center",
                  color: "#6b7280",
                  padding: "2rem 0",
                }}
              >
                No messages yet. Send your first message!
              </p>
            ) : (
              <div
                style={{
                  display: "flex",
                  flexDirection: "column",
                  gap: "1rem",
                }}
              >
                {messages.map((msg) => (
                  <div
                    key={msg.id}
                    style={{
                      padding: "1rem",
                      border: "1px solid #e5e7eb",
                      borderRadius: "0.5rem",
                    }}
                  >
                    <div
                      style={{
                        display: "flex",
                        justifyContent: "space-between",
                        alignItems: "flex-start",
                        marginBottom: "0.5rem",
                      }}
                    >
                      <p style={{ color: "#1f2937" }}>{msg.content}</p>
                      <span style={{ fontSize: "0.875rem", color: "#6b7280" }}>
                        {new Date(msg.timestamp).toLocaleTimeString()}
                      </span>
                    </div>
                    <span
                      style={{
                        padding: "0.25rem 0.5rem",
                        borderRadius: "9999px",
                        fontSize: "0.75rem",
                        backgroundColor:
                          msg.status === "Sent" ? "#dcfce7" : "#fef9c3",
                        color: msg.status === "Sent" ? "#15803d" : "#854d0e",
                      }}
                    >
                      {msg.status}
                    </span>
                  </div>
                ))}
              </div>
            )}
          </div>
        </div>
      </div>
    </div>
  );
};

export default MessageDashboard;
