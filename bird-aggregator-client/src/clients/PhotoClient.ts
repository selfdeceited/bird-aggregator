import { ImageProps } from '../components/Gallery/types'
import { fetchAs } from '.'

type PhotoByIdResponse = { photo: ImageProps }

export async function fetchPhoto(photoId: string): Promise<ImageProps> {
	const { photo } = await fetchAs<PhotoByIdResponse>(`/api/gallery/photo/${photoId}`)
	return photo
}
