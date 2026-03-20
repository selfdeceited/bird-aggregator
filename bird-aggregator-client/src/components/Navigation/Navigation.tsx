import * as React from 'react'
import * as Popover from '@radix-ui/react-popover'
import { useEffect, useState } from 'react'
import styled from 'styled-components'
import { ChevronsUpDown } from 'lucide-react'

import { Bird, fetchAllBirds } from '../../clients/GalleryClient'
import { LinkResponse, fetchAs } from '../../clients'
import { filterBird } from './BirdSelect'
import { BirdLink, GithubLink, ImageHostingLink, LifeListLink, MapLink, RootPageLink } from './links'
import { NavBar, NavGroup, NavDivider, NavButton } from './NavStyled'

const ComboboxContent = styled(Popover.Content)`
  background: #fff;
  border-radius: 4px;
  box-shadow: 0 4px 16px rgba(0, 0, 0, 0.2);
  width: 280px;
  z-index: 100;
`

const ComboboxInput = styled.input`
  width: 100%;
  padding: 8px 10px;
  border: none;
  border-bottom: 1px solid #e0e0e0;
  font-size: 13px;
  outline: none;
  box-sizing: border-box;
`

const ComboboxList = styled.div`
  max-height: 260px;
  overflow-y: auto;
`

const ComboboxItem = styled.div`
  display: flex;
  justify-content: space-between;
  padding: 7px 10px;
  font-size: 13px;
  cursor: pointer;
  &:hover { background: #f0f4f8; }
`

const ComboboxItemLatin = styled.span`
  color: #777;
  font-style: italic;
`

export const Navigation: React.FC = () => {
	const [birds, setBirds] = useState<Bird[]>([])
	const [githubLink, setGithubLink] = useState('')
	const [imageHostingLink, setImageHostingLink] = useState('')
	const [userName, setUserName] = useState('')
	const [selectedBird, setSelectedBird] = useState<Bird>({ id: 0, name: '', latin: '' })
	const [open, setOpen] = useState(false)
	const [query, setQuery] = useState('')

	useEffect(() => {
		const fetchEverything = async () => {
			const fetchedGithubLink = await fetchAs<LinkResponse>('/api/links/github')
			setGithubLink(fetchedGithubLink)

			const fetchedImageHostingLink = await fetchAs<LinkResponse>('/api/links/hosting')
			setImageHostingLink(fetchedImageHostingLink)

			const fetchedUserName = await fetchAs<LinkResponse>('/api/links/user')
			setUserName(fetchedUserName)

			const fetchedBirds = await fetchAllBirds()
			setBirds(fetchedBirds)
			const [initiallySelectedBird] = fetchedBirds
			setSelectedBird(initiallySelectedBird)
		}
		void fetchEverything()
	}, [])

	const filteredBirds = query ? birds.filter(b => filterBird(query, b)) : birds

	const handleSelect = (bird: Bird) => {
		setSelectedBird(bird)
		setOpen(false)
		setQuery('')
	}

	return (
		<NavBar>
			<NavGroup>
				<div className="logo"/>
				<RootPageLink userName={userName}/>
				<NavDivider/>
				<MapLink/>
				<LifeListLink/>
				<NavDivider/>
				<div className="bird-info-select">
					<span>Find specific bird: &nbsp;</span>
					<Popover.Root open={open} onOpenChange={setOpen}>
						<Popover.Trigger asChild>
							<NavButton>
								{selectedBird?.name || 'Select bird'} <ChevronsUpDown size={13}/>
							</NavButton>
						</Popover.Trigger>
						<Popover.Portal>
							<ComboboxContent sideOffset={4} align="start">
								<ComboboxInput
									autoFocus
									placeholder="Search birds..."
									value={query}
									onChange={(e: React.ChangeEvent<HTMLInputElement>) => setQuery(e.target.value)}
								/>
								<ComboboxList>
									{filteredBirds.map(bird => (
										<ComboboxItem key={bird.id} onMouseDown={() => handleSelect(bird)}>
											<span>{bird.name}</span>
											<ComboboxItemLatin>{bird.latin}</ComboboxItemLatin>
										</ComboboxItem>
									))}
								</ComboboxList>
							</ComboboxContent>
						</Popover.Portal>
					</Popover.Root>
					{selectedBird?.id ? (
						<>
							<span style={{ display: 'inline-block', width: 4 }}/>
							<BirdLink birdId={selectedBird.id}/>
						</>
					) : null}
				</div>
			</NavGroup>
			<NavGroup $align="right">
				<GithubLink githubLink={githubLink}/>
				<ImageHostingLink imageHostingLink={imageHostingLink}/>
			</NavGroup>
		</NavBar>
	)
}
