import { WikiData } from '../components/BirdPage/BirdGallery'
import { fetchAs } from '.'

type WikiDataResponse = { data:WikiData }
type BirdsResponse = {birds: Bird[]}
export type Bird = { id: number; name: string; latin: string }


export async function fetchWikiData(birdId: string): Promise<WikiData> {
	const { data } = await fetchAs<WikiDataResponse>(`/api/birds/info/${birdId}`)
	return data
}


export async function fetchAllBirds(): Promise<Bird[]> {
	const { birds } = await fetchAs<BirdsResponse>('/api/birds')
	return birds
}
