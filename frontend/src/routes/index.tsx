import { createFileRoute } from '@tanstack/react-router'
import { Button } from '@/components/ui/button'

export const Route = createFileRoute('/')({
  component: Index,
})

function Index() {
  return (
    <section className="w-full py-12 md:py-24 lg:py-32">
      <div className="container flex flex-col items-center justify-center space-y-4 text-center px-4 md:px-6">
        <div className="space-y-3">
          <h1 className="text-3xl font-bold tracking-tighter sm:text-4xl md:text-5xl lg:text-6xl/none">
            Your Next Great Book
            <br className="hidden sm:inline" />
            Starts Here
          </h1>
          <p className="mx-auto max-w-[700px] text-muted-foreground text-base sm:text-lg md:text-xl">
            Welcome to Book Publisher, where your literary journey begins. We help authors bring their stories to life.
          </p>
        </div>
        <div className="flex flex-wrap items-center justify-center gap-4">
          <Button size="lg">Get Started</Button>
          <Button size="lg" variant="outline">Learn More</Button>
        </div>
      </div>
    </section>
  )
}