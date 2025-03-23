import { useQuery } from '@tanstack/react-query'
import { createFileRoute } from '@tanstack/react-router'

export const Route = createFileRoute('/query')({
  component: RouteComponent,
})

function RouteComponent() {

  const {data} = useQuery({
    queryKey: ['test'],
    queryFn: getTest
  })

  return(
    <>
      <div>{JSON.stringify(data)}</div>
    </>
  )
}

const getTest = async () => {
  const response = await fetch('http://localhost:5094/api/v1/covers')
  return await response.json()
}