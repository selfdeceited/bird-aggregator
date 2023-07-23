import * as Blueprint from '@blueprintjs/core'
import * as React from 'react'

import { Alignment, Navbar } from '@blueprintjs/core'

import { Bird, fetchAllBirds } from '../../clients/GalleryClient'
import { BirdLink, GithubLink, ImageHostingLink, LifeListLink, MapLink, RootPageLink, TripListLink } from './links'
import { LinkResponse, fetchAs } from '../../clients'
import { filterBird, renderBird } from './BirdSelect'
import { useEffect, useState } from 'react'
import { Select } from '@blueprintjs/labs'

export const Navigation: React.FC = () => {
	const [birds, setBirds] = useState<Bird[]>([])
	const [githubLink, setGithubLink] = useState('')
	const [imageHostingLink, setImageHostingLink] = useState('')
	const [userName, setUserName] = useState('')
	const [selectedBird, setSelectedBird] = useState<Bird>({ id: 0, name: '', latin: '' })

	useEffect(() => {
		const fetchEverything: () => Promise<void> = async () => {
			const fetchedGithubLink = await fetchAs<LinkResponse>('/api/links/github')
			setGithubLink(fetchedGithubLink)

			const fetchedImageHostingLink = await fetchAs<LinkResponse>('/api/links/hosting')
			setImageHostingLink(fetchedImageHostingLink)

			const fetchedUserName = await fetchAs<LinkResponse>('/api/links/user')
			setUserName(fetchedUserName)

			const fetchedBirds = await fetchAllBirds()
			setBirds(fetchedBirds)

			const [ initiallySelectedBird ] = fetchedBirds
			setSelectedBird(initiallySelectedBird)
		}
		void fetchEverything()
	}, [])

	return (
		<Navbar>
			<Navbar.Group align={Alignment.LEFT}>
				<div className="logo"></div>
				<RootPageLink userName={userName}/>
				<Navbar.Divider />
				<MapLink/>
				<TripListLink/>
				<LifeListLink/>
				<Navbar.Divider />
				<div className="bird-info-select">
					<div className="inline-block">Find specific bird: &nbsp;</div>
					<Select<Bird>
						items={birds}
						noResults={<Blueprint.MenuItem disabled text="No results." />}
						itemPredicate={filterBird}
						itemRenderer={renderBird}
						onItemSelect={setSelectedBird}
					></Select>

					<Blueprint.Button
						text={
							selectedBird?.name ?? ''
						}
						rightIcon="double-caret-vertical"
					/>

					{selectedBird ? (
						<span>
							<span className="small-space"></span>
							<BirdLink birdId={selectedBird.id}/>
						</span>
					) : null}
				</div>
			</Navbar.Group>
			<Navbar.Group align={Alignment.RIGHT}>
				<GithubLink githubLink={githubLink}/>
				<ImageHostingLink imageHostingLink={imageHostingLink}/>
			</Navbar.Group>
		</Navbar>
	)
}
