import * as React from 'react'

import { Link } from 'react-router-dom'
import { ShortBirdInfo } from './types'

interface BirdPopupProps {
	birds: ShortBirdInfo[]
	photoUrl: string
}

export const BirdPopup: React.FC<BirdPopupProps> = (props: BirdPopupProps) => {
	const imagePreview = props.birds.length > 2 ? null : (
		<div>
			<img
				src={props.photoUrl}
				className="marker-thumbnail"
				alt={props.birds.map(x => x.name).join(',')}/>
		</div>)

	return (<div>
		<p>Birds found:</p>
		{
			props.birds
			// .filter(x => x.id > 0)
				.map(x => (<Link
					key={x.id}
					to={`/birds/${x.id}`}
					role="button"
					className="bp3-button bp3-minimal bp3-icon-arrow-right display-block small-reference">
					{x.name}
				</Link>
				))
		}
		{imagePreview}
	</div>)
}
