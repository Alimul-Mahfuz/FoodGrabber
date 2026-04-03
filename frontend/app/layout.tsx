import type { Metadata } from "next";
import "./globals.css";

export const metadata: Metadata = {
  title: "FoodGrabber",
  description: "Frontend for the FoodGrabber ordering platform.",
};

import Navbar from "@/components/Navbar";

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <html lang="en">
      <body>
        <Navbar />
        {children}
      </body>
    </html>
  );
}
