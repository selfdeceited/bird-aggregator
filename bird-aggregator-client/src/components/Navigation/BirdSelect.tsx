import { Bird } from '../../clients/GalleryClient'

export const filterBird = (query: string, bird: Bird): boolean =>
	`${bird.name.toLowerCase()} ${bird.latin.toLowerCase()}`.includes(query.toLowerCase())
