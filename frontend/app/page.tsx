export default function HomePage() {
  return (
    <main className="page">
      <section className="hero">
        <p className="eyebrow">FoodGrabber Frontend</p>
        <h1>Next.js is now scaffolded inside this repository.</h1>
        <p className="copy">
          Build the customer app here while keeping the ASP.NET API isolated in
          the existing `src/` backend structure.
        </p>
      </section>

      <section className="panel">
        <h2>Suggested next steps</h2>
        <ul>
          <li>Connect this app to your auth and menu endpoints.</li>
          <li>Set up shared API environment variables.</li>
          <li>Build the browsing, cart, and checkout flows.</li>
        </ul>
      </section>
    </main>
  );
}
