import * as axios from '../../http.adapter'

import { BirdGalleryMapStyled, BirdInfoStyled, BirdPageGalleryStyled, BirdPageStyled } from './BirdPageStyled'
import React, { FC, useEffect, useState } from 'react'

import { Gallery } from '../Gallery/Gallery'
import { MapContainer } from '../Map/Map'
import { WikiDescription } from './Wikipedia/WikiDescription'
import { WikiImage } from './Wikipedia/WikiImage'

interface IWikiData {
	name: string
	wikiInfo: string
	imageUrl: string
}

interface ParamMatchedProps {
	match: {
		params: {
			id: number
		}
	}
}

/* eslint-disable @typescript-eslint/no-unsafe-member-access, @typescript-eslint/no-unsafe-assignment*/
const fetchWikiInfo = async (birdId: number, setWikiData: (data: IWikiData) => void): Promise<void> => {
	const { data: wikiData } = await axios.get(`/api/birds/info/${birdId}`)
	const extract = JSON.parse(wikiData.wikiInfo)
	const html = extract.query.pages[Object.keys(extract.query.pages)[0]].extract

	const div = document.createElement('div')
	div.innerHTML = html

	try {
		const chapters = fillChapters(div)
		setWikiData({ name: wikiData.name, wikiInfo: chapters.outerHTML, imageUrl: wikiData.imageUrl })
	} catch (e) {
		// eslint-disable-next-line no-console
		console.error(e)
	}
}

const fillChapters = (div: HTMLDivElement): HTMLDivElement => {
	const finalContainer = document.createElement('div')
	const paragraphs = Array.prototype.slice.call(div.getElementsByTagName('p')).filter(x => x.innerHTML.length > 2)
	for (const paragraph of paragraphs) {
		finalContainer.append(paragraph)
		if (finalContainer.innerHTML.length > 1000) {
			break
		}
	}

	return finalContainer
}
/* eslint-enable @typescript-eslint/no-unsafe-member-access, @typescript-eslint/no-unsafe-assignment*/

export const BirdGallery: FC<ParamMatchedProps> = props => {
	const {
		match: {
			params: { id: birdId },
		},
	} = props
	const [wikiData, setWikiData] = useState<IWikiData>({
		name: '',
		wikiInfo: '',
		imageUrl: '',
	})

	useEffect(() => {
		// eslint-disable-next-line @typescript-eslint/no-floating-promises
		fetchWikiInfo(birdId, setWikiData)
	}, [birdId])

	// todo: think how to show in better on mobile
	return (
		<BirdPageStyled>
			<BirdPageGalleryStyled>
				<Gallery
					seeFullGalleryLink={false}
					urlToFetch={`/api/gallery/bird/${birdId}`}
					showImageCaptions={false}
				/>
			</BirdPageGalleryStyled>
			<BirdInfoStyled>
				{wikiData ? <WikiImage imageUrl={wikiData.imageUrl} /> : null}
				{wikiData ? <WikiDescription birdName={wikiData.name} wikiInfo={wikiData.wikiInfo} /> : null}
				<BirdGalleryMapStyled>
					<h4>Observations on map</h4>
					<MapContainer embedded birdId={birdId} />
				</BirdGalleryMapStyled>

				<div className="wiki-info hide">
					<h4>Voice sample</h4>
					<p>(todo)</p>
					<code>http://www.xeno-canto.org/article/153</code>
				</div>
			</BirdInfoStyled>
		</BirdPageStyled>
	)
}
