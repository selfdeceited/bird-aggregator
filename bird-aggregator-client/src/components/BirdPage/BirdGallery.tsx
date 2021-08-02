import * as axios from '../../http.adapter'

import { BirdInfoStyled, BirdPageGalleryStyled, BirdPageStyled } from './BirdPageStyled'
import React, { FC, useEffect, useState } from 'react'

import { GalleryWrap } from '../Gallery/GalleryWrap'
import { MapWrap } from '../Map/MapWrap'
import { WikiDescription } from './Wikipedia/WikiDescription'
import { WikiImage } from './Wikipedia/WikiImage'

interface IWikiData {
	name: string
	wikiInfo: string
	imageUrl: string
}

const fetchWikiInfo = (props: any, setWikiData: (data: IWikiData) => void) => {
	axios.get('/api/birds/info/' + props.match.params.id).then(res => {
		const wikiData = res.data
		const extract = JSON.parse(wikiData.wikiInfo)
		const html = extract.query.pages[Object.keys(extract.query.pages)[0]].extract

		const div = document.createElement('div')
		div.innerHTML = html

		try {
			const chapters = fillChapters(div)
			setWikiData({ name: res.data.name, wikiInfo: chapters.outerHTML, imageUrl: res.data.imageUrl })
		}
		// tslint:disable-next-line: no-empty
		catch { }
	})
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

export const BirdGallery: FC<any> = props => {
	const [wikiData, setWikiData] = useState<IWikiData>({
		name: '',
		wikiInfo: '',
		imageUrl: '',
	})

	useEffect(() => fetchWikiInfo(props, setWikiData), [props])


	// todo: think how to show in better on mobile
	return (
		<BirdPageStyled>
			<BirdPageGalleryStyled>
				<GalleryWrap
					seeFullGalleryLink={false}
					urlToFetch={'/api/gallery/bird/' + props.match.params.id}
				/>
			</BirdPageGalleryStyled>
			<BirdInfoStyled>
				{
					wikiData ? <WikiImage imageUrl={wikiData.imageUrl}/> : null
				}
				{
					wikiData ? <WikiDescription name={wikiData.name} wikiInfo={wikiData.wikiInfo}/> : null
				}
				<div className="wiki-info">
					<h4>Occurences on map</h4>
					<MapWrap embedded
						birdId={props.match.params.id}
					/>
				</div>

				<div className="wiki-info hide">
					<h4>Voice sample</h4>
					<p>(todo)</p>
					<code>http://www.xeno-canto.org/article/153</code>
				</div>
			</BirdInfoStyled>
		</BirdPageStyled>
	)
}
