import * as React from 'react'
import Lightbox from 'react-image-lightbox'
import { Link } from 'react-router-dom'
import { Image } from './BirdImage'

interface IBirdLightboxProps {
	photos: Image[]
	index: number
	onClose: any
}


export const BirdLightbox: React.FC<IBirdLightboxProps> = props => {
	const [index, setIndex] = React.useState(props.index)
	const images = props.photos.map(x => x.original)

	React.useEffect(() => {
		setIndex(props.index)
	}, [props])

	function zip<T, U>(list: T[], ...lists: U[][]): (U|T)[][] {
		return lists.reduce((previousValue: (U|T)[][], currentValue: (U|T)[]) =>
			previousValue.map((p, i) => [...p, currentValue[i]]), list.map(x => [x]) as (U|T)[][]
		)
	}

	const photoTitles = zip(
		props.photos[index].caption.split(','),
		props.photos[index].birdIds)

	const getTitle = () => <div>
		{
			photoTitles.map((arr: (string | number)[], i: number) => <Link
				key={arr[0]}
				className="big-link lightbox-caption"
				to={`/birds/${arr[1]}`}>
				{arr[0]}
				{i === photoTitles.length - 1 ? '' : ','}
			</Link>)
		}
	</div>

	const getDescription = () => <div>
		<span>{props.photos[index].text}</span>
		<Link
			key={props.photos[index].id}
			to={'/photos/' + props.photos[index].id}
			role="button"
			className="bp3-button bp3-minimal bp3-icon-arrow-right lightbox-photo-link">
        Visit photo page
		</Link>
	</div>

	const getLightbox = () => <div>
		<Lightbox
			mainSrc={images[index]}
			nextSrc={images[(index + 1) % images.length]}
			prevSrc={images[(index + images.length - 1) % images.length]}
			onCloseRequest={props.onClose}
			onMovePrevRequest={() =>
				setIndex((index + images.length - 1) % images.length)
			}
			onMoveNextRequest={() =>
				setIndex((index + 1) % images.length)
			}
			imageTitle={getTitle()}
			imageCaption={getDescription()}
		/>
	</div>

	return index === null ? null : getLightbox()
}
