import { createFileRoute } from '@tanstack/react-router'

export const Route = createFileRoute('/about')({
  component: About,
})

function About() {
  return (
    <section className="w-full py-12 md:py-24 lg:py-32">
      <div className="container px-4 md:px-6">
        <div className="mx-auto max-w-[800px] space-y-12">
          <div className="space-y-4">
            <h1 className="text-3xl font-bold tracking-tighter sm:text-4xl md:text-5xl">
              About Us
            </h1>
            <p className="text-base sm:text-lg text-muted-foreground md:text-xl">
              Empowering authors to share their stories with the world.
            </p>
          </div>
          <div className="grid gap-8 sm:grid-cols-2">
            <div className="space-y-3">
              <h2 className="text-xl font-bold">Our Mission</h2>
              <p className="text-muted-foreground">
                We believe every story deserves to be told. Our platform provides the tools and support needed to bring your book from manuscript to published work.
              </p>
            </div>
            <div className="space-y-3">
              <h2 className="text-xl font-bold">Our Vision</h2>
              <p className="text-muted-foreground">
                To create a world where publishing is accessible to all, where great stories find their audience, and where authors can thrive.
              </p>
            </div>
          </div>
          <div className="space-y-4">
            <h2 className="text-xl font-bold">Tech Stack</h2>
            <div className="grid gap-2 sm:grid-cols-2 lg:grid-cols-3">
              <div className="flex items-center space-x-2 rounded-lg border p-4">
                <div>
                  <h3 className="font-medium">Frontend Framework</h3>
                  <p className="text-sm text-muted-foreground">React 19 + Vite 6</p>
                </div>
              </div>
              <div className="flex items-center space-x-2 rounded-lg border p-4">
                <div>
                  <h3 className="font-medium">Type Safety</h3>
                  <p className="text-sm text-muted-foreground">TypeScript</p>
                </div>
              </div>
              <div className="flex items-center space-x-2 rounded-lg border p-4">
                <div>
                  <h3 className="font-medium">Styling</h3>
                  <p className="text-sm text-muted-foreground">Tailwind CSS + Shadcn UI</p>
                </div>
              </div>
              <div className="flex items-center space-x-2 rounded-lg border p-4">
                <div>
                  <h3 className="font-medium">Routing</h3>
                  <p className="text-sm text-muted-foreground">TanStack Router</p>
                </div>
              </div>
              <div className="flex items-center space-x-2 rounded-lg border p-4">
                <div>
                  <h3 className="font-medium">Data Management</h3>
                  <p className="text-sm text-muted-foreground">TanStack Query</p>
                </div>
              </div>
              <div className="flex items-center space-x-2 rounded-lg border p-4">
                <div>
                  <h3 className="font-medium">Form Handling</h3>
                  <p className="text-sm text-muted-foreground">TanStack Form</p>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </section>
  )
}