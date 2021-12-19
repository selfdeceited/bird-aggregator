import { BirdLinkStyled, TimestampStyled } from './GalleryStyled'

import { FlickrLinkStyled } from './FlickrLinkStyled'
import { Link } from 'react-router-dom'
import React from 'react'
import moment from 'moment'

type Props = {
	birdIds: number[]
	birdNames: string
	dateTaken: string
	hostingLink: string
}

function zip<T, U>(listT: T[], listU: U[]): ([T, U])[] {
	return listT.map((t: T, i: number) => [t, listU[i]])
}

export const BirdLink: React.FC<Props> = (
	{ birdIds, birdNames, dateTaken, hostingLink }) => {
	const photoTitles = zip(birdNames.split(','), birdIds)
	return <BirdLinkStyled>
		{
			photoTitles.map((_: ([string, number]), i: number) => {
				const [birdName, birdId] = _
				return <Link key={birdName}
					to={`/birds/${birdId}`}
					style={{ color: '#aedaf9', textDecoration: 'none' }}
				>{birdName}{i === photoTitles.length - 1 ? '' : ','}</Link>
			})
		}
		<FlickrLinkStyled href={hostingLink} target="_blank">
			show source...
		</FlickrLinkStyled>
		<TimestampStyled>
			{moment(dateTaken).format('LLLL')}
		</TimestampStyled>
	</BirdLinkStyled>
}
