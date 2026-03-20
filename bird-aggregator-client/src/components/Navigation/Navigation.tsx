import * as Popover from '@radix-ui/react-popover'
import * as React from 'react'
import { Bird, fetchAllBirds } from '../../clients/GalleryClient'
import { BirdLink, GithubLink, ImageHostingLink, LifeListLink, MapLink, RootPageLink } from './links'
import { LinkResponse, fetchAs } from '../../clients'
import { NavBar, NavButton, NavDivider, NavGroup } from './NavStyled'
import { useCallback, useEffect, useState } from 'react'
import { ChevronsUpDown } from 'lucide-react'
import { filterBird } from './BirdSelect'
import styled from 'styled-components'

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

type BirdItemProps = { bird: Bird; onSelect: (b: Bird) => void }

const BirdItem: React.FC<BirdItemProps> = ({ bird, onSelect }) => {
	const handleMouseDown = useCallback((): void => {
		onSelect(bird)
	}, [onSelect, bird])
	return (
		<ComboboxItem onMouseDown={handleMouseDown}>
			<span>{bird.name}</span>
			<ComboboxItemLatin>{bird.latin}</ComboboxItemLatin>
		</ComboboxItem>
	)
}

export const Navigation: React.FC = () => {
	const [birds, setBirds] = useState<Bird[]>([])
	const [githubLink, setGithubLink] = useState('')
	const [imageHostingLink, setImageHostingLink] = useState('')
	const [userName, setUserName] = useState('')
	const [selectedBird, setSelectedBird] = useState<Bird>({ id: 0, name: '', latin: '' })
	const [isOpen, setIsOpen] = useState(false)
	const [query, setQuery] = useState('')

	useEffect(() => {
		const fetchEverything = async (): Promise<void> => {
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

	const handleSelect = useCallback((bird: Bird): void => {
		setSelectedBird(bird)
		setIsOpen(false)
		setQuery('')
	}, [])

	const handleQueryChange = useCallback((e: React.ChangeEvent<HTMLInputElement>): void => {
		setQuery(e.target.value)
	}, [])

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
					<Popover.Root open={isOpen} onOpenChange={setIsOpen}>
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
									onChange={handleQueryChange}
								/>
								<ComboboxList>
									{filteredBirds.map(bird => (
										<BirdItem key={bird.id} bird={bird} onSelect={handleSelect} />
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
