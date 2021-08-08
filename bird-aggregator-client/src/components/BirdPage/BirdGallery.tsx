import * as axios from '../../http.adapter'

import { BirdGalleryMapStyled, BirdInfoStyled, BirdPageGalleryStyled, BirdPageStyled } from './BirdPageStyled'
import React, { FC, useEffect, useState } from 'react'

import { Gallery } from '../Gallery/Gallery'
import { MapWrap } from '../Map/Map'
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

const fetchWikiInfo = async (birdId: number, setWikiData: (data: IWikiData) => void) => {
	const { data: wikiData } = await axios.get('/api/birds/info/' + birdId)
	const extract = JSON.parse(wikiData.wikiInfo)
	const html = extract.query.pages[Object.keys(extract.query.pages)[0]].extract

	const div = document.createElement('div')
	div.innerHTML = html

	try {
		const chapters = fillChapters(div)
		setWikiData({ name: wikiData.name, wikiInfo: chapters.outerHTML, imageUrl: wikiData.imageUrl })
	} catch { }
}

const fillChapters = (div: HTMLDivElement): HTMLDivElement => {
	const finalContainer = document.createElement('div')
	const paragraphs = Array.prototype.slice.call(div.getElementsByTagName('p')).filter(x => x.innerHTML.length > 2)
	// tslint:disable-next-line: prefer-for-of
	for (let i = 0; i < paragraphs.length; i++) {
		finalContainer.append(paragraphs[i])
		if (finalContainer.innerHTML.length > 1000) {
			break
		}
	}

	return finalContainer
}

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
		fetchWikiInfo(birdId, setWikiData)
	}, [birdId])

	// todo: think how to show in better on mobile
	return (
		<BirdPageStyled>
			<BirdPageGalleryStyled>
				<Gallery
					seeFullGalleryLink={false}
					urlToFetch={'/api/gallery/bird/' + birdId}
					showImageCaptions={false}
				/>
			</BirdPageGalleryStyled>
			<BirdInfoStyled>
				{wikiData ? <WikiImage imageUrl={wikiData.imageUrl} /> : null}
				{wikiData ? <WikiDescription name={wikiData.name} wikiInfo={wikiData.wikiInfo} /> : null}
				<BirdGalleryMapStyled>
					<h4>Observations on map</h4>
					<MapWrap embedded birdId={birdId} />
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
