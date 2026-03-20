import * as Popover from '@radix-ui/react-popover'
import React, { useEffect, useState } from 'react'
import styled from 'styled-components'
import { Link } from 'react-router-dom'
import { Calendar, MapPin, ArrowRight } from 'lucide-react'

import { MapContainer } from './Map/Map'
import { YearlyLifeList } from './YearlyLifeList'
import { fetchLifelist } from '../clients/LifeListClient'
import moment from 'moment'

export interface ILifeListDto {
	birdId: string
	name: string
	dateMet: string
	location: string
	photoId: string
}

const Container = styled.article`
  margin-top: 10px;
  margin-left: 20px;
`

const IconButton = styled.button`
  display: inline-flex;
  align-items: center;
  gap: 5px;
  padding: 5px 10px;
  border-radius: 3px;
  border: 1px solid #d0d0d0;
  background: transparent;
  font-size: 14px;
  cursor: pointer;
  &:hover { background: #f0f4f8; }
`

const MapIconButton = styled.button`
  display: inline-flex;
  align-items: center;
  padding: 4px;
  border-radius: 3px;
  border: none;
  background: transparent;
  cursor: pointer;
  color: #5c7080;
  &:hover { background: rgba(167, 182, 194, 0.3); }
`

const PopoverContent = styled(Popover.Content)`
  background: #fff;
  border-radius: 4px;
  box-shadow: 0 4px 16px rgba(0, 0, 0, 0.2);
  z-index: 20;
`

const StyledTable = styled.table`
  border-collapse: collapse;
  width: 100%;
  tbody tr:nth-child(odd) { background: #f6f7f9; }
  th, td {
    padding: 6px 10px;
    text-align: left;
    font-size: 14px;
  }
  th { font-weight: 600; border-bottom: 2px solid #d0d0d0; }
  td { border-bottom: 1px solid #e8e8e8; }
`

const BirdLinkButton = styled(Link)`
  display: inline-flex;
  align-items: center;
  padding: 3px;
  border-radius: 3px;
  text-decoration: none;
  color: #5c7080;
  &:hover { background: rgba(167, 182, 194, 0.3); }
`

export const LifeList: React.FC = () => {
	const [lifelist, setLifelist] = useState<ILifeListDto[]>([])
	const [yearlyOpen, setYearlyOpen] = useState(false)

	useEffect(() => {
		const fillLifelist = async () => {
			const firstOccurrences = await fetchLifelist()
			setLifelist(firstOccurrences)
		}
		void fillLifelist()
	}, [])

	const LifeListPopover: React.FC<{ photoId: string }> = ({ photoId }) => photoId ? (
		<Popover.Root>
			<Popover.Trigger asChild>
				<MapIconButton><MapPin size={14}/></MapIconButton>
			</Popover.Trigger>
			<Popover.Portal>
				<PopoverContent sideOffset={4}>
					<MapContainer embedded photoId={photoId}/>
				</PopoverContent>
			</Popover.Portal>
		</Popover.Root>
	) : null

	return (
		<Container>
			<h2 className="show-mobile">Life list</h2>
			<div>
				<Popover.Root open={yearlyOpen} onOpenChange={setYearlyOpen}>
					<Popover.Trigger asChild>
						<IconButton>
							<Calendar size={14}/>&nbsp;&nbsp;Yearly statistics
						</IconButton>
					</Popover.Trigger>
					<Popover.Portal>
						<PopoverContent sideOffset={4}>
							<YearlyLifeList/>
						</PopoverContent>
					</Popover.Portal>
				</Popover.Root>
			</div>
			<StyledTable>
				<thead>
					<tr>
						<th className="hide-mobile">&nbsp;</th>
						<th>Species</th>
						<th>Date</th>
						<th><span className="hide-mobile">Location</span></th>
					</tr>
				</thead>
				<tbody>
					{lifelist.map((x: ILifeListDto, i: number) => (
						<tr key={i}>
							<td className="hide-mobile">{i + 1}</td>
							<td className="bird-column">
								{x.name}
								<BirdLinkButton key={x.photoId} to={`/photos/${x.photoId}`}>
									<ArrowRight size={14}/>
								</BirdLinkButton>
							</td>
							<td className="date-column">{moment(x.dateMet).format('YYYY MM DD')}</td>
							<td>
								<span className="hide-mobile">{x.location}&nbsp;&nbsp;</span>
								<LifeListPopover photoId={x.photoId}/>
							</td>
						</tr>
					))}
				</tbody>
			</StyledTable>
		</Container>
	)
}
