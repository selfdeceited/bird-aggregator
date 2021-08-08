import * as Blueprint from '@blueprintjs/core'
import * as React from 'react'
import * as axios from '../../http.adapter'

import { BirdLink, GithubLink, ImageHostingLink, LifeListLink, MapLink, RootPageLink, TripListLink } from './links/'
import { BirdSelect, filterBird, renderBird } from './BirdSelect'
import { useEffect, useState } from 'react'

export type Bird = { id: number; name: string; latin: string }

export const Navbar: React.FC = () => {
	const [birds, setBirds] = useState([] as Bird[])
	const [githubLink, setGithubLink] = useState('')
	const [imageHostingLink, setImageHostingLink] = useState('')
	const [userName, setUserName] = useState('')
	const [selectedBird, setSelectedBird] = useState({ id: 0, name: '', latin: '' } as Bird)

	/* eslint-disable @typescript-eslint/no-unsafe-member-access, @typescript-eslint/no-unsafe-assignment*/
	useEffect(() => {
		const fetchEverything: () => Promise<void> = async () => {
			const { data: fetchedGithubLink } = await axios.get('/api/links/github')
			setGithubLink(fetchedGithubLink)

			const { data: fetchedImageHostingLink } = await axios.get('/api/links/hosting')
			setImageHostingLink(fetchedImageHostingLink)

			const { data: fetchedUserName } = await axios.get('/api/links/user')
			setUserName(fetchedUserName)

			const { data: { birds: fetchedBirds } } = await axios.get('/api/birds')
			setBirds(fetchedBirds)

			const [ initiallySelectedBird ] = fetchedBirds
			setSelectedBird(initiallySelectedBird)
		}
		// eslint-disable-next-line @typescript-eslint/no-floating-promises
		fetchEverything()
	}, [])

	/* eslint-enable @typescript-eslint/no-unsafe-member-access, @typescript-eslint/no-unsafe-assignment*/

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
