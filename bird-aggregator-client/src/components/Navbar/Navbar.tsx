import * as Blueprint from '@blueprintjs/core'
import * as React from 'react'

import { Bird, fetchAllBirds } from '../../clients/GalleryClient'
import { BirdLink, GithubLink, ImageHostingLink, LifeListLink, MapLink, RootPageLink, TripListLink } from './links/'
import { BirdSelect, filterBird, renderBird } from './BirdSelect'
import { LinkResponse, fetchAs } from '../../clients'
import { useEffect, useState } from 'react'

export const Navbar: React.FC = () => {
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
		<nav className="bp3-navbar bp3-fixed-top">
			<div className="bp3-navbar-group bp3-align-left">
				<div className="logo"></div>
				<RootPageLink userName={userName}/>
				<span className="bp3-navbar-divider"></span>
				<MapLink/>
				<TripListLink/>
				<LifeListLink/>
				<span className="bp3-navbar-divider"></span>
				<div className="bird-info-select">
					<div className="inline-block">Find specific bird: &nbsp;</div>
					<BirdSelect
						items={birds}
						noResults={<Blueprint.MenuItem disabled text="No results." />}
						itemPredicate={filterBird}
						itemRenderer={renderBird}
						onItemSelect={setSelectedBird}
					>
						<Blueprint.Button
							text={
								selectedBird?.name ?? ''
							}
							rightIcon="double-caret-vertical"
						/>
					</BirdSelect>
					{selectedBird ? (
						<span>
							<span className="small-space"></span>
							<BirdLink birdId={selectedBird.id}/>
						</span>
					) : null}
				</div>
			</div>
			<div className="bp3-navbar-group bp3-align-right">
				<GithubLink githubLink={githubLink}/>
				<ImageHostingLink imageHostingLink={imageHostingLink}/>
			</div>
		</nav>
	)
}
